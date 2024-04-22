using CacheEditor;
using EpsilonLib.Commands;
using EpsilonLib.Dialogs;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace MapVariantFixer
{
    class MapVariantFixerViewModel : Screen
    {
        private IShell _shell;
        private ICacheFile _cacheFile;
        private string _output;
        private bool _inProgress;
        private Dictionary<int, string> _061_TagRemapping;
        private readonly Dictionary<string, string> ObjeRenames = new Dictionary<string, string>()
        {
            { "objects\\equipment\\instantcover_equipment\\instantcover_equipment.eqip", "objects\\equipment\\instantcover_equipment\\instantcover_equipment_mp.eqip" },
            { "objects\\levels\\multi\\cyberdyne\\cyber_monitor_med\\cyber_monitor.scen", "objects\\levels\\solo\\020_base\\monitor_med\\monitor_med.scen" },
            { "objects\\levels\\multi\\s3d_turf\\s3d_turf_turf_crate_large\\s3d_turf_turf_crate_large.bloc", "objects\\levels\\multi\\s3d_turf\\turf_crate_large\\turf_crate_large.bloc" }
        };

        public MapVariantFixerViewModel(IShell shell, ICacheFile cacheFile)
        {
            _shell = shell;
            _cacheFile = cacheFile;

            DisplayName = "Map Variant Fixer";
            StartCommand = new DelegateCommand(Start, () => Files.Count > 0 && !_inProgress);
            ClearCommand = new DelegateCommand(ClearFiles, () => Files.Count > 0 && !_inProgress);

            Files.CollectionChanged += Files_CollectionChanged;

            var sandboxMapsDir = new DirectoryInfo(Path.Combine(_cacheFile.File.Directory.FullName, "..\\data\\map_variants"));
            if (sandboxMapsDir.Exists)
                AddFilesRecursive(sandboxMapsDir);
        }

        public ObservableCollection<string> Files { get; } = new ObservableCollection<string>();
        public string CacheFilePath => _cacheFile.File.FullName;
        public DelegateCommand StartCommand { get; }
        public DelegateCommand ClearCommand { get; }

        public string Output
        {
            get => _output;
            set
            {
                SetAndNotify(ref _output, value);
            }
        }

        public bool InProgress
        {
            get => _inProgress;
            set
            {
                if (SetAndNotify(ref _inProgress, value))
                {
                    StartCommand.RaiseCanExecuteChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        internal async void Start()
        {
            InProgress = true;
            Output = string.Empty;

            using (var progress = _shell.CreateProgressScope())
            {
                try
                {
                    var backupFilePath = Path.Combine(Directory.GetCurrentDirectory(), GetBackupFileName());

                    WriteLog($"creating backup '{backupFilePath}'...");
                    await CreateBackupAsync(backupFilePath);

                    var baseCache = _cacheFile.Cache;

                    for(int i = 0; i < Files.Count; i++)
                    {
                        var filePath = Path.GetFullPath(Files[i]);
                        progress.Report($"Fixing map variant '{filePath}'...", false, (i+1) / (float)Files.Count);
                        await Task.Run(() => FixMapVariantBlocking(baseCache, filePath));
                    }

                    WriteLog("done.");
                }
                catch (Exception ex)
                {
                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        Message = "One or more map variants failed. Check the output for details."
                    };
                    _shell.ShowDialog(alert);

                    WriteLog(ex.ToString());
                }
                finally
                {
                    InProgress = false;
                }
            }
        }

        void FixMapVariantBlocking(GameCache baseCache, string filePath)
        {
            WriteLog($"fixing '{filePath}'...");
            var sandboxMapFile = new FileInfo(filePath);
            using (var stream = sandboxMapFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var reader = new EndianReader(stream);
                var writer = new EndianWriter(stream);

                Fix061Endianess(stream);

                var blf = new Blf(baseCache.Version, baseCache.Platform);
                blf.Read(reader);

                if (blf.MapVariant == null)
                    return;

                if (blf.MapVariantTagNames == null)
                {
                    WriteLog("converting from 0.6.1 format...");
                    Convert061MapVariant(blf);
                }

                var palette = blf.MapVariant.MapVariant.Quotas;
                for (int i = 0; i < palette.Length; i++)
                {
                    if (palette[i].ObjectDefinitionIndex == -1)
                        continue;

                    var name = blf.MapVariantTagNames.Names[i].Name;
                    CachedTag tag;

                    if (baseCache.TagCache.TryGetTag(name, out tag))
                    {
                        continue;
                    }
                    else
                    {
                        string newName = "";

                        if (name.StartsWith("ms30"))
                            newName = name.Substring(5);
                        else
                            newName = $"ms30\\{name}";
                        
                        if (baseCache.TagCache.TryGetTag(newName, out tag))
                        {
                            blf.MapVariantTagNames.Names[i].Name = newName;
                            WriteLog($"Fixed name '{newName}'");
                        }
                        else if (ObjeRenames.TryGetValue(name, out var reName))
                        {
                            blf.MapVariantTagNames.Names[i].Name = reName;
                            WriteLog($"Fixed name '{reName}'");
                        }
                        else
                        {
                            throw new Exception($"Missing tag {name} in the base cache. Reach out to a dev for help.");
                        }
                    }
                }

                if(blf.EndOfFile == null)
                {
                    WriteLog("fixing EOF chunk...");
                    blf.EndOfFile = new BlfChunkEndOfFile()
                    {
                        Signature = new Tag("_eof"),
                        Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkEndOfFile), blf.Version, baseCache.Platform),
                        MajorVersion = 1,
                        MinorVersion = 1,
                    };
                    blf.ContentFlags |= BlfFileContentFlags.EndOfFile;
                }

                WriteLog("saving file...");
                stream.Position = 0;
                if (!blf.Write(writer))
                    throw new Exception("failed to write blf");
            }
        }

        private void Fix061Endianess(Stream stream)
        {
            var deserializer = new TagDeserializer(CacheVersion.HaloOnlineED, CachePlatform.Original);
            var serializer = new TagSerializer(CacheVersion.HaloOnlineED, CachePlatform.Original);

            var reader = new EndianReader(stream, EndianFormat.BigEndian);
            var writer = new EndianWriter(stream, EndianFormat.LittleEndian);
            var readerContext = new DataSerializationContext(reader);
            var writerContext = new DataSerializationContext(writer);

            // check this is actually a big endian chunk header (this could also be true for h3 files, but that's their fault)
            if(reader.ReadTag() != "_blf")
            {
                stream.Position = 0;
                return;
            }

            WriteLog("Fixing chunk endianess...");

            reader.BaseStream.Position = 0;
            // fix the chunk headers
            while (true)
            {
                var pos = reader.BaseStream.Position;
                var header = (BlfChunkHeader)deserializer.Deserialize(readerContext, typeof(BlfChunkHeader));
               
                writer.BaseStream.Position = pos;
                serializer.Serialize(writerContext, header);
                if (header.Signature == "_eof")
                    break;

                reader.BaseStream.Position += header.Length - typeof(BlfChunkHeader).GetSize();
            }

            // fix the BOM
            stream.Position = 0xC;
            writer.Format = EndianFormat.LittleEndian;
            writer.Write((short)-2);
            stream.Position = 0;
        }

        private void Convert061MapVariant(Blf blf)
        {
            if(_061_TagRemapping == null)
            {
                _061_TagRemapping = Properties.Resources._061_mapping
                                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Split(','))
                                .Where(delim => delim.Length > 1)
                                .Select(delim => new { TagIndex = System.Convert.ToInt32(delim[0].Trim(), 16), Name = delim[1].Trim() })
                                .ToDictionary(x => x.TagIndex, y => y.Name);
            }
            
            blf.MapVariantTagNames = new BlfMapVariantTagNames()
            {
                Signature = new Tag("tagn"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfMapVariantTagNames), blf.Version, CachePlatform.Original),
                MajorVersion = 1,
                MinorVersion = 0,
                Names = Enumerable.Range(0, 256).Select(x => new TagName()).ToArray(),
            };
            blf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;

            var mapVariant = blf.MapVariant.MapVariant;
            for(int i = 0; i < mapVariant.Quotas.Length; i++)
            {
                var tagIndex = mapVariant.Quotas[i].ObjectDefinitionIndex;
                if (tagIndex == -1)
                    continue;
                if(_061_TagRemapping.TryGetValue(tagIndex, out string name))
                {
                    blf.MapVariantTagNames.Names[i] = new TagName() { Name = name };
                    WriteLog($"added tag name entry 0x{tagIndex:X04} -> {name}");
                }
            }
        }

        private async Task CreateBackupAsync(string outputPath)
        {
            var zipArchive = new ZipArchive(File.Create(outputPath), ZipArchiveMode.Create);
            var zipEntries = new HashSet<string>();

            foreach (var filePath in Files)
            {
                var sandboxMapFile = new FileInfo(filePath);
                var relativePath = sandboxMapFile.FullName.Replace(sandboxMapFile.Directory.FullName.Replace(sandboxMapFile.Directory.Name, ""), "");
                if (!zipEntries.Add(relativePath))
                    throw new InvalidOperationException($"Could not create backup, conflicting folder names '${relativePath}'");

                var entry = zipArchive.CreateEntry(relativePath);
                using (var entryStream = entry.Open())
                {
                    using (var inputStream = sandboxMapFile.Open(FileMode.Open, FileAccess.Read))
                        await inputStream.CopyToAsync(entryStream);
                }
            }
            zipArchive.Dispose();
        }

        private string GetBackupFileName()
        {
            return $"map_variant_backup_{DateTime.Now.ToFileTime()}.zip";
        }

        internal void ClearFiles()
        {
            Files.Clear();
            Output = string.Empty;
        }

        internal void AddFiles(string[] files)
        {
            foreach (var filePath in files)
            {
                FileAttributes attr = File.GetAttributes(filePath);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    AddFilesRecursive(new DirectoryInfo(filePath));
                }
                else
                {
                    AddFile(new FileInfo(filePath));
                }
            }
        }

        private void AddFilesRecursive(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles("sandbox.map", SearchOption.AllDirectories))
                AddFile(file);
        }

        private void AddFile(FileInfo file)
        {
            if (Files.Contains(file.FullName))
                return;

            Files.Add(file.FullName);
            StartCommand.RaiseCanExecuteChanged();
        }

        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ClearCommand.RaiseCanExecuteChanged();
            StartCommand.RaiseCanExecuteChanged();
        }

        private void WriteLog(string output)
        {
            Application.Current.Dispatcher.Invoke(() => Output += $"{output}\n");
        }
    }
}
