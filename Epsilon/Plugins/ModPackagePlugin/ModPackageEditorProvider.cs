using CacheEditor;
using EpsilonLib.Dialogs;
using EpsilonLib.Editors;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TagTool.Cache;

namespace ModPackagePlugin
{
    [Export(typeof(IEditorProvider))]
    class ModPackageEditorProvider : IEditorProvider
    {
        private readonly ICacheEditingService _editingService;

        public string DisplayName => "Mod Package";

        public static Guid EditorId = new Guid("{BECDF82F-CF23-4DBD-BE7A-A44DBF943BFB}");

        public Guid Id => EditorId;

        public IReadOnlyList<string> FileExtensions => new[] { ".pak" };

        [ImportingConstructor]
        public ModPackageEditorProvider(ICacheEditingService editingService)
        {
            _editingService = editingService;
        }

        public async Task OpenFileAsync(IShell shell, string fileName)
        {
            var file = new FileInfo(fileName);
            FileInfo baseCacheFile = new FileInfo(Path.Combine(file.Directory.FullName, "..\\maps\\tags.dat"));
            if (!baseCacheFile.Exists)
            {
                if (!FileDialogs.RunBrowseCacheDialog(out baseCacheFile))
                    return;
            }

            if (!file.Exists)
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    DisplayName = "File Not Found",
                    Message = $"Mod package not found at this location:",
                    SubMessage = fileName
                };
                shell.ShowDialog(alert);

                return;
            }

            try
            {
                var cache = await Task.Run(() => new GameCacheModPackage((GameCacheHaloOnlineBase)GameCache.Open(baseCacheFile), file));
                shell.ActiveDocument = (IScreen)_editingService.CreateEditor(new ModPackageCacheFile(file, cache));
            }
            catch
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    DisplayName = "Failed to Open",
                    Message = $"An error occurred while opening this mod package; it may be incompatible or corrupted.",
                    SubMessage = fileName
                };
                shell.ShowDialog(alert);

                return;
            }
        }
    }
}
