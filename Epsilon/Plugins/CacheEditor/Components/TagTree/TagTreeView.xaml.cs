using EpsilonLib.Shell.TreeModels;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CacheEditor;

namespace CacheEditor.Components.TagTree
{
    /// <summary>
    /// Interaction logic for TagTreeView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TagTreeView : UserControl
    {
        public TagTreeView()
        {
            InitializeComponent();
            Loaded += TagTreeView_Loaded;

            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyUpEvent, new KeyEventHandler(TagTreeWindowKeyUp));
        }

        private void TagTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBox.Focus();
            Keyboard.Focus(SearchBox);
        }

        private void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (e.OriginalSource as DependencyObject).FindAncestors<TreeViewItem>().FirstOrDefault();
            if (item != null)
            {
                item.Focus();
            }
            else
            {
                ((TreeView)sender).Focus();
            }
        }

        private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;

            if (tvi == null || e.Handled) return;

            tvi.IsExpanded = !tvi.IsExpanded;
            //tvi.IsSelected = false;
            e.Handled = true;
        }

        private void TagTreeWindowKeyUp(object sender, KeyEventArgs e)
        {
            // ctrl-T to focus Tag Tree Search

            if ((e.Key == Key.T && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) || (e.Key == Key.LeftCtrl && e.KeyboardDevice.IsKeyDown(Key.T)))
            {
                TagTreeViewModel tagTreeViewModel = (TagTreeViewModel)DataContext;
                if (tagTreeViewModel != null && IsVisible)
                {
                    SearchBox.Focus();
                    Keyboard.Focus(SearchBox);
                    SearchBox.Select(0, SearchBox.Text.Length);
                    e.Handled = true;
                }
            }
        }
    }
}
