using Epsilon.Pages;
using EpsilonLib.Commands;
using Stylet;
using System.ComponentModel.Composition;

namespace Epsilon.Commands
{
    [ExportCommandHandler]
    class ShowAboutCommandHandler : ICommandHandler<ShowAboutCommand>
    {
        private IWindowManager _windowManager;

        [ImportingConstructor]
        public ShowAboutCommandHandler(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void ExecuteCommand(Command command)
        {
            _windowManager.ShowDialog(new AboutViewModel());
        }

        public void UpdateCommand(Command command)
        {

        }
    }
}
