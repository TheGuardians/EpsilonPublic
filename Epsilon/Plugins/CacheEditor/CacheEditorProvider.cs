using EpsilonLib.Dialogs;
using EpsilonLib.Editors;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TagTool.Cache;

namespace CacheEditor
{
    [Export(typeof(IEditorProvider))]
    class CacheEditorProvider : IEditorProvider
    {
        private readonly ICacheEditingService _editingService;

        public string DisplayName => "Cache File";

        public Guid Id => new Guid("{444EDD8C-F984-40BF-9CC6-4FEF104609E1}");

        public IReadOnlyList<string> FileExtensions => new[] { ".dat", ".map" };

        [ImportingConstructor]
        public CacheEditorProvider(ICacheEditingService editingService)
        {
            _editingService = editingService;
        }

        async Task IEditorProvider.OpenFileAsync(IShell shell, string fileName)
        {
            var file = new FileInfo(fileName);

            if(!file.Exists)
            {
                var error = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    DisplayName = "File Not Found",
                    Message = $"Tag cache not found at this location:",
                    SubMessage = $"{fileName}"
                };
                shell.ShowDialog(error);

                return;
            }

            var cache = await Task.Run(() => GameCache.Open(file));
            shell.ActiveDocument = new CacheEditorViewModel(shell, _editingService, CreateCacheFileDocument(file, cache));
        }

        ICacheFile CreateCacheFileDocument(FileInfo file, GameCache cache)
        {
            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, cache.Version))
                return new HaloOnlineCacheFile(file, cache);
            else
                return new GenericCacheFile(file, cache);
        }
    }
}
