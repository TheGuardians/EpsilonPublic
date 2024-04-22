using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock;

namespace CacheEditor
{
    /// <summary>
    /// Interaction logic for CacheEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CacheEditorView : UserControl
    {
        public CacheEditorView()
        {
            InitializeComponent();

            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(OnWindowKeyDown));
        }

        private void DockingManager_ActiveContentChanged(object sender, EventArgs e)
        {
            var dockingManager = (DockingManager)sender;
            Debug.WriteLine($"Active Content {dockingManager.ActiveContent}");
        }
        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            // ctrl-W to close current tag

            if ((e.Key == Key.W && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) || (e.Key == Key.LeftCtrl && e.KeyboardDevice.IsKeyDown(Key.W)))
            {
                var cacheViewModel = DataContext as CacheEditorViewModel;
                if (cacheViewModel.IsActive)
                {
                    if (cacheViewModel.ActiveItem is TagEditorViewModel currentTagViewModel && currentTagViewModel.CloseCommand.CanExecute(null))
                    {
                        currentTagViewModel.CloseCommand.Execute(null);
                    }
                    e.Handled = true;
                }
            }
        }
    }
}