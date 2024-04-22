using EpsilonLib.Commands;

namespace CacheEditor.Commands
{
    public abstract class ShowToolWindowCommandHandlerBase<T> : ICommandHandler<T> where T : CommandDefinition
    {
        private ICacheEditingService _cacheEditingService;

        public ShowToolWindowCommandHandlerBase(ICacheEditingService cacheEditingService)
        {
            _cacheEditingService = cacheEditingService;
        }

        public abstract string ToolName { get; }

        public ICacheEditor CurrentEditor => _cacheEditingService.ActiveCacheEditor;
        public ICacheEditorTool GetTool() => CurrentEditor?.GetTool(ToolName);

        public void ExecuteCommand(Command command)
        {
            var tool = GetTool();
            if(tool != null)
                tool.Show(!tool.IsVisible, true);
        }

        public void UpdateCommand(Command command)
        {
            var tool = GetTool();

            command.IsVisible = tool != null;

            if (tool != null)
                command.IsChecked = tool.IsVisible;
        }
    }
}
