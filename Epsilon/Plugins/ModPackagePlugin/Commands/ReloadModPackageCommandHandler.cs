using CacheEditor;
using EpsilonLib.Commands;
using EpsilonLib.Settings;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;

namespace ModPackagePlugin.Commands
{
    [ExportCommandHandler]
    class ReloadModPackageCommandHandler : ICommandHandler<ReloadModPackageCommand>
    {
        private readonly Lazy<IShell> _shell;
        private readonly ICacheEditingService _editingService;
        private readonly IFileHistoryService _fileHistory;
        private readonly ISettingsService _settingsService;

        [ImportingConstructor]
        public ReloadModPackageCommandHandler(Lazy<IShell> shell, ICacheEditingService editingService, IFileHistoryService fileHistory, ISettingsService settingsService)
        {
            _shell = shell;
            _editingService = editingService;
            _fileHistory = fileHistory;
            _settingsService = settingsService;
        }

        public async void ExecuteCommand(Command command)
        {
            string baseCachePath = "";
            if (_editingService.ActiveCacheEditor?.CacheFile is ModPackageCacheFile)
            {
                baseCachePath = ((_editingService.ActiveCacheEditor.CacheFile.Cache as GameCacheModPackage)
                .BaseCacheReference as GameCacheHaloOnline).TagsFile.ToString();
            }
            else
                return;

            var shell = _shell.Value;
            var currentMod = shell.ActiveDocument;

            FileInfo modInfo = _editingService.ActiveCacheEditor.CacheFile.File;
            FileInfo baseInfo = new FileInfo(baseCachePath);

            string currentTagName = _editingService.ActiveCacheEditor.CurrentTag?.ToString();
            List<string> openTagsList = _editingService.ActiveCacheEditor.GetOpenTagNames();

            using (var progress = shell.CreateProgressScope())
            {
                progress.Report($"Reloading \"{modInfo.Name}\"...");
                shell.ActiveDocument.RequestClose();
                
                var modCache = await Task.Run(() => new GameCacheModPackage((GameCacheHaloOnlineBase)GameCache.Open(baseInfo), modInfo));
                shell.ActiveDocument = (IScreen)_editingService.CreateEditor(new ModPackageCacheFile(modInfo, modCache));

                ReopenTags(openTagsList, currentTagName);
            }
        }

        private void ReopenTags(List<string> openTagsList, string currentTagName)
        {
            foreach (var name in openTagsList)
            {
                _editingService.ActiveCacheEditor.CacheFile.Cache.TagCache.TryGetCachedTag(name, out CachedTag tag);
                if (tag != null && !tag.IsInGroup("bitm"))
                    _editingService.ActiveCacheEditor.OpenTag(tag);
            }

            var conductor = _editingService.ActiveCacheEditor as Conductor<IScreen>.Collection.OneActive;
            conductor.ActivateItem(conductor.Items.FirstOrDefault(tag => tag.ToString() == currentTagName));
        }

        public void UpdateCommand(Command command)
        {
            var packageCache = _editingService?.ActiveCacheEditor?.CacheFile?.Cache as GameCacheModPackage;
            command.IsVisible = packageCache != null;
        }
    }
}
