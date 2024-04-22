using CacheEditor.Commands;
using EpsilonLib.Commands;
using System.ComponentModel.Composition;

namespace CacheEditor
{
    [ExportCommandHandler]
    class ShowTagExplorerCommandHandler : ShowToolWindowCommandHandlerBase<ShowTagExplorerCommand>
    {
        [ImportingConstructor]
        public ShowTagExplorerCommandHandler(ICacheEditingService cacheEditingService) : base(cacheEditingService)
        {
        }

        public override string ToolName => TagExplorerViewModel.ToolName;
    }
}
