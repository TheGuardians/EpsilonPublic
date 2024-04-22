using CacheEditor;
using CacheEditor.Commands;
using EpsilonLib.Commands;
using System.ComponentModel.Composition;

namespace TagToolShellPlugin.Commands
{
    [ExportCommandHandler]
    class ShowShellWindowCommandHandler : ShowToolWindowCommandHandlerBase<ShowShellWindowCommand>
    {
        [ImportingConstructor]
        public ShowShellWindowCommandHandler(ICacheEditingService editingService) : base(editingService)
        {
        }

        public override string ToolName => CommandShellToolViewModel.ToolName;
    }
}
