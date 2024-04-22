using CacheEditor;
using EpsilonLib.Commands;
using EpsilonLib.Options;
using Shared;
using System;
using System.ComponentModel.Composition;

namespace ServerJsonEditor
{
    [ExportCommandHandler]
    class ShowServerJsonEditorCommandHandler : ICommandHandler<ShowServerJsonEditorCommand>
    {
        private readonly Lazy<IShell> _shell;
        private readonly ICacheEditingService _cacheEditingService;
        private readonly IOptionsService _optionsService;

        [ImportingConstructor]
        public ShowServerJsonEditorCommandHandler(Lazy<IShell> shell, ICacheEditingService cacheEditingService, IOptionsService optionsService)
        {
            _shell = shell;
            _cacheEditingService = cacheEditingService;
            _optionsService = optionsService;
        }

        public void ExecuteCommand(Command command)
        {
            var editor = _cacheEditingService.ActiveCacheEditor;
            if (editor == null)
            {
                try
                {
                    // if no cache open, enter "open file" dialog
                }
                catch (Exception ex)
                {
                    // "cache file invalid"
                }
                return;
            }

            _shell.Value.ActiveDocument = new ServerJsonEditorViewModel(_shell.Value, editor.CacheFile);
        }

        public void UpdateCommand(Command command)
        {
            //command.IsVisible = _cacheEditingService.ActiveCacheEditor != null &&
            //    _cacheEditingService.ActiveCacheEditor.CacheFile.Cache.Version == TagTool.Cache.CacheVersion.HaloOnlineED;
            command.IsVisible = false;
        }
    }
}
