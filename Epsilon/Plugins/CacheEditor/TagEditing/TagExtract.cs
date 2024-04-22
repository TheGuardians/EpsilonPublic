using Shared;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Commands.Models;
using TagTool.Commands.Sounds;
using System.Collections.Generic;
using System.Linq;

namespace CacheEditor.TagEditing
{
    [Export]
    class TagExtract
    {
        private IShell _shell;

        public TagExtract(IShell shell)
        {
            _shell = shell;
        }

        public void ExtractBitmap(GameCache cache, CachedTag tag)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                using (var stream = cache.OpenCacheRead())
                {
                    var bitmap = cache.Deserialize<Bitmap>(stream, tag);
                    ExtractBitmap(cache, tag, bitmap, dialog.SelectedPath);
                }
            }
        }

        public void ExportJMS(GameCache cache, CachedTag tag, string type)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                var filename = tag.Name.Split('\\').Last() + "_" + type + ".jms";
                var fullpath = Path.Combine(dialog.SelectedPath, tag.Name.Split('\\').Last(), filename);


                using (var stream = cache.OpenCacheRead())
                {
                    var hlmt = cache.Deserialize<Model>(stream, tag);
                    ExportJMSCommand exportJMSCommand = new ExportJMSCommand(cache, hlmt);
                    exportJMSCommand.Execute(new List<string>() { type, fullpath });
                }

                _shell.StatusBar.ShowStatusText($"JMS Exported to \"{fullpath}\"");
            }
        }

        public void ExtractSound(GameCache cache, CachedTag tag)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                var fullpath = dialog.SelectedPath;

                using (var stream = cache.OpenCacheRead())
                {
                    var sound = cache.Deserialize<Sound>(stream, tag);

                    //if (sound.PitchRanges?[0].Permutations.Count > 1)
                    //    fullpath = Path.Combine(dialog.SelectedPath, tag.Name.Split('\\').Last());

                    //Directory.CreateDirectory(fullpath);

                    ExtractSoundCommand extractSoundCommand = new ExtractSoundCommand(cache, tag, sound);
                    extractSoundCommand.Execute(new List<string>() { fullpath });
                }

                _shell.StatusBar.ShowStatusText($"Sound files extracted to \"{fullpath}\"");
            }
        }

        private void ExtractBitmap(GameCache cache, CachedTag tag, Bitmap bitmap, string directory = "bitmaps")
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var ddsOutDir = directory;
            string name;
            if (tag.Name != null)
            {
                var split = tag.Name.Split('\\');
                name = split[split.Length - 1];
            }
            else
                name = tag.Index.ToString("X8");
            if (bitmap.Images.Count > 1)
            {
                ddsOutDir = Path.Combine(directory, name);
                Directory.CreateDirectory(ddsOutDir);
            }

            for (var i = 0; i < bitmap.Images.Count; i++)
            {
                var bitmapName = (bitmap.Images.Count > 1) ? i.ToString() : name;
                bitmapName += ".dds";
                var outPath = Path.Combine(ddsOutDir, bitmapName);

                var ddsFile = BitmapExtractor.ExtractBitmap(cache, bitmap, i, tag.Name);

                if (ddsFile == null)
                    throw new Exception("Invalid bitmap data");

                using (var fileStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                {
                    ddsFile.Write(writer);
                }
            }

            _shell.StatusBar.ShowStatusText("Bitmap Extracted");
        }
    }
}
