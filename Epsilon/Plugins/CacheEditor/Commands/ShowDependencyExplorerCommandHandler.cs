using EpsilonLib.Commands;
using System.ComponentModel.Composition;

namespace CacheEditor.Commands
{
    [ExportCommandHandler]
    class ShowDependnecyExplorerCommandHandler : ShowToolWindowCommandHandlerBase<ShowDependencyExplorerCommand>
    {
        [ImportingConstructor]
        public ShowDependnecyExplorerCommandHandler(ICacheEditingService cacheEditingService) : base(cacheEditingService)
        {
        }

        public override string ToolName => Components.DependencyExplorer.DependencyExplorerViewModel.ToolName;
    }
}
