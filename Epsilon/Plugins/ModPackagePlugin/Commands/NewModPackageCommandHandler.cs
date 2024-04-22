using CacheEditor;
using EpsilonLib.Commands;
using EpsilonLib.Settings;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.Gen3;
using TagTool.Cache.HaloOnline;
using TagTool.IO;
using static TagTool.IO.ConsoleHistory;

namespace ModPackagePlugin.Commands
{
    [ExportCommandHandler]
    class NewModPackageCommandHandler : ICommandHandler<NewModPackageCommand>
    {
        private readonly Lazy<IShell> _shell;
        private readonly ICacheEditingService _editingService;
        private readonly IFileHistoryService _fileHistory;
        private readonly ISettingsService _settingsService;

        [ImportingConstructor]
        public NewModPackageCommandHandler(Lazy<IShell> shell, ICacheEditingService editingService, IFileHistoryService fileHistory, ISettingsService settingsService)
        {
            _shell = shell;
            _editingService = editingService;
            _fileHistory = fileHistory;
            _settingsService = settingsService;
        }

        public async void ExecuteCommand(Command command)
        {
            var shell = _shell.Value;
            var baseCachePath = _settingsService.GetCollection("General").Get("DefaultTagCache", "");
            FileInfo baseCacheFile;

            if (File.Exists(baseCachePath))
                baseCacheFile = new FileInfo(baseCachePath);
            else if (!FileDialogs.RunBrowseCacheDialog(out baseCacheFile))
                return;

            var directory = Path.GetFullPath($"{baseCacheFile.Directory.FullName}\\..\\mods");
            if (!FileDialogs.RunSaveDialog(out FileInfo modpackageFile, initialDirectory: directory))
                return;

            using (var progress = shell.CreateProgressScope())
            {
                progress.Report("Loading base cache file...");

                var baseCache = (GameCacheHaloOnline)GameCache.Open(baseCacheFile);
                var cache = await CreateModPackageCacheAsync(baseCache, modpackageFile, progress);

                shell.ActiveDocument = (IScreen)_editingService.CreateEditor(new ModPackageCacheFile(modpackageFile, cache));
            }
        }

        private async Task<GameCacheModPackage> CreateModPackageCacheAsync(GameCacheHaloOnline baseCache, FileInfo file, IProgressReporter progress)
        {
            GameCacheModPackage modCache = await Task.Run(() => CreateAndInitializePackage(baseCache, progress));
            
            modCache.BaseModPackage.Metadata.Name = file.Name.Split('.')[0];
            modCache.BaseModPackage.Header.ModifierFlags |= ModifierFlags.multiplayer;

            progress.Report("Creating package file...");
            await Task.Run(() => modCache.SaveModPackage(file));

            await _fileHistory.RecordFileOpened(ModPackageEditorProvider.EditorId, file.FullName);

            return modCache;
        }

        private static GameCacheModPackage CreateAndInitializePackage(GameCacheHaloOnline baseCache, IProgressReporter progress)
        {
            var modCache = new GameCacheModPackage(baseCache);

            var referenceStream = new MemoryStream(); // will be reused by all base caches
            TagTool.Commands.Modding.CreateModPackageCommand.BuildInitialTagCache(baseCache, modCache, referenceStream);

            referenceStream.Position = 0;

            Dictionary<int, string> tagNames = new Dictionary<int, string>();

            foreach (var tag in baseCache.TagCache.NonNull())
                tagNames[tag.Index] = tag.Name;

            modCache.BaseModPackage.TagCachesStreams.Add(new ExtantStream(referenceStream));
            modCache.BaseModPackage.CacheNames.Add("default");
            modCache.BaseModPackage.TagCacheNames.Add(tagNames);

            modCache.SetActiveTagCache(0);

            return modCache;
        }

        public void UpdateCommand(Command command)
        {

        }
    }
}
