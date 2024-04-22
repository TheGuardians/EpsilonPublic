using EpsilonLib.Commands;
using System.ComponentModel.Composition;

namespace CacheEditor.Commands
{
    [ExportCommandHandler]
    class ShowCommandLogCommandHandler : ShowToolWindowCommandHandlerBase<ShowCommandLogCommand>
    {
        [ImportingConstructor]
        public ShowCommandLogCommandHandler(ICacheEditingService cacheEditingService) : base(cacheEditingService)
        {
        }

        public override string ToolName => Components.CommandLog.CommandLogViewModel.ToolName;
    }
}
