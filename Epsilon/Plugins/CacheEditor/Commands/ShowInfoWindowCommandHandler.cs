using EpsilonLib.Commands;
using System.ComponentModel.Composition;

namespace CacheEditor.Commands
{
    [ExportCommandHandler]
    class ShowInfoWindowCommandHandler : ShowToolWindowCommandHandlerBase<ShowInfoWindowCommand>
    {
        [ImportingConstructor]
        public ShowInfoWindowCommandHandler(ICacheEditingService cacheEditingService) : base(cacheEditingService)
        {
        }

        public override string ToolName => Components.Info.CacheInfoToolViewModel.ToolName;
    }
}
