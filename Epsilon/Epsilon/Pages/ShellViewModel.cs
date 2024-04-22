using Epsilon.Components;
using EpsilonLib.Dialogs;
using EpsilonLib.Editors;
using EpsilonLib.Logging;
using EpsilonLib.Menus;
using EpsilonLib.Shell;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;

namespace Epsilon.Pages
{
    [Export]
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        private readonly IWindowManager _windowManager;
        private readonly IMenuFactory _menuFactory;
        private readonly Lazy<IEditorService> _editorService;

        [ImportingConstructor]
        public ShellViewModel(
            [ImportMany] IEnumerable<IEditorProvider> editorProviders,
            IWindowManager windowManager,
            IMenuFactory menuFactory, 
            Lazy<IEditorService> editorService)
        {
            _windowManager = windowManager;
            _menuFactory = menuFactory;
            _editorService = editorService;

            StatusBar = new StatusBarModel();

            var assemblyInfo = Assembly.GetExecutingAssembly().GetName();
            Title = $"{assemblyInfo.Name}";
            MainMenu = new BindableCollection<MenuItemViewModel>(_menuFactory.GetMenuBar(StandardMenus.MainMenu));
            RefreshMainMenu();
        }

        public string Title { get;  }

        public IObservableCollection<MenuItemViewModel> MainMenu { get; }

        public IStatusBar StatusBar { get; }

        public IObservableCollection<IScreen> Documents => Items;

        public IScreen ActiveDocument
        {
            get => ActiveItem;
            set
            {
                ActiveItem = value;
                RefreshMainMenu();
                NotifyOfPropertyChange(nameof(ActiveDocument));
            }
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            HandleCommandLine();

            RefreshMainMenu();
        }

        public override void ActivateItem(IScreen item)
        {
            if (item != null && item.ScreenState == ScreenState.Closed)
                return;

            base.ActivateItem(item);
        }

        private void RefreshMainMenu()
        {
            foreach (var menu in MainMenu)
                menu.Refresh();
        }

        public IProgressReporter CreateProgressScope()
        {
            return new ShellProgressReporter(StatusBar);
        }

        public bool? ShowDialog(object viewModel)
        {
            return _windowManager.ShowDialog(viewModel);
        }

        private async void HandleCommandLine()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length < 2)
                return;

            try
            {
                await _editorService.Value.OpenFileAsync(args[1]);
            }
            catch(Exception ex)
            {
                Logger.Error($"Failed to open file from command line '{args[1]}'\n{ex}");
            }
        }
        public async void OnDroppedFiles(string[] files)
        {
            var file = files[0];


            try
            {
                await _editorService.Value.OpenFileAsync(file);
            }
            catch (Exception ex)
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    Message = "An error occurred while opening the file. Check the log for details."
                };
                ShowDialog(alert);

                Logger.Error(ex.ToString());
            }
        }
    }
}
