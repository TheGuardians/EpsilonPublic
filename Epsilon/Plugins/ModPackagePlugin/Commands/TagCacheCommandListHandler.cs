using CacheEditor;
using EpsilonLib.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using TagTool.Cache;

namespace ModPackagePlugin.Commands
{
    [ExportCommandHandler]
    class TagCacheCommandListHandler : 
        ICommandHandler<TagCacheCommandList>, 
        ICommandListPopulator<TagCacheCommandList>
    {
        private readonly Lazy<ICacheEditingService> _cacheEditingService;

        [ImportingConstructor]
        public TagCacheCommandListHandler(Lazy<ICacheEditingService> cacheEditingService)
        {
            _cacheEditingService = cacheEditingService;
        }

        private ICacheEditor ActiveEditor => _cacheEditingService.Value?.ActiveCacheEditor;

        public void ExecuteCommand(Command command)
        {
            var index = (int)command.Tag;
            if (ActiveEditor.CacheFile.Cache is GameCacheModPackage packageCache)
            {
                packageCache.SetActiveTagCache(index);
                ActiveEditor.Reload();
            }
        }

        public IEnumerable<Command> PopulateCommandList(Command command)
        {
            if(ActiveEditor.CacheFile.Cache is GameCacheModPackage packageCache)
            {
                var cacheNames = packageCache.BaseModPackage.CacheNames;
                for (int i = 0; i < cacheNames.Count; i++)
                    yield return new Command(command.Definition) { RequiresUpdate = true, Tag = i, DisplayText = cacheNames[i] };
            }
        }

        public void UpdateCommand(Command command)
        {
            var packageCache = ActiveEditor?.CacheFile?.Cache as GameCacheModPackage;

            if(command.Tag == null)
                command.IsVisible = packageCache != null;

            if (packageCache != null)
            {
                if (command.Tag is int i)
                    command.IsChecked = packageCache.GetCurrentTagCacheIndex() == i;
            }
        }
    }
}
