using Epsilon.Options;
using EpsilonLib.Commands;
using EpsilonLib.Options;
using Stylet;
using System.ComponentModel.Composition;

namespace Epsilon.Commands
{
    [ExportCommand]
    class ShowOptionsCommand : CommandDefinition
    {
        public override string Name => "Tools.Options";

        public override string DisplayText => "Options";
    }

    [ExportCommandHandler]
    class ShowOptionsCommandHandler : ICommandHandler<ShowOptionsCommand>
    {
        private IWindowManager _windowManager;
        private IOptionsService _optionsService;
        private OptionsViewModel _viewModel;

        [ImportingConstructor]
        public ShowOptionsCommandHandler(IWindowManager windowManager, IOptionsService optionsService)
        {
            _windowManager = windowManager;
            _optionsService = optionsService;
            _viewModel = new OptionsViewModel(optionsService);
        }

        public void ExecuteCommand(Command command)
        {
            _windowManager.ShowDialog(_viewModel);
        }

        public void UpdateCommand(Command command)
        {
            
        }
    }
}
