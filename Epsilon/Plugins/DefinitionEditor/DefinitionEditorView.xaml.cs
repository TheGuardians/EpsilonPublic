using EpsilonLib.Menus;
using EpsilonLib.Utils;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TagStructEditor.Fields;

namespace DefinitionEditor
{
    /// <summary>
    /// Interaction logic for TagEditorView.xaml
    /// </summary>

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DefinitionEditorView : UserControl
    {
        public DefinitionEditorView()
        {
            InitializeComponent();

            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyUpEvent, new KeyEventHandler(TagDefWindowKeyUp));
        }

        private void TagDefWindowKeyUp(object sender, KeyEventArgs e)
        {
            // ctrl-F to focus Definition Search

            if ((e.Key == Key.F && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) || (e.Key == Key.LeftCtrl && e.KeyboardDevice.IsKeyDown(Key.F)))
            {
                DefinitionEditorViewModel definitionViewModel = (DefinitionEditorViewModel)DataContext;
                if (definitionViewModel != null && IsVisible)
                {
                    SearchBox.Focus();
                    Keyboard.Focus(SearchBox);
                    e.Handled = true;
                }
            }

            // ctrl-S to save

            else if ((e.Key == Key.S && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) || (e.Key == Key.LeftCtrl && e.KeyboardDevice.IsKeyDown(Key.S)))
            {
                DefinitionEditorViewModel definitionViewModel = (DefinitionEditorViewModel)DataContext;
                if (definitionViewModel != null && IsVisible)
                {
                    definitionViewModel.SaveCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        private void structContainer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (!(e.OriginalSource is Visual))
                return;

            // If it's a textbox, just use the default context menu for now.
            if (VisualTreeHelpers.FindAncestors<TextBox>((Visual)e.OriginalSource).FirstOrDefault() != null)
                return;

            // Search the ancestors for the closest field
            var field = VisualTreeHelpers.FindAncestorDataContext<IField>((UIElement)e.OriginalSource);
            if (field == null)
                return;

            if (!(DataContext is DefinitionEditorViewModel vm))
                return;

            // Build the context menu      
            var menu = new EMenu();
            if (!vm.PopulateContextMenu(menu, field))
                return;

            var ctxMenu = new ContextMenu();
            menu.PopulateTopLevelMenu(menu, ctxMenu.Items);
            if (ctxMenu.Items.Count == 0)
                return;

           
            ctxMenu.Style = Application.Current.TryFindResource(typeof(ContextMenu)) as Style;
            ctxMenu.Placement = PlacementMode.RelativePoint;
            ctxMenu.HorizontalOffset = e.CursorLeft;
            ctxMenu.VerticalOffset = e.CursorTop;
            ctxMenu.PlacementTarget = (UIElement)e.OriginalSource;
            ctxMenu.IsOpen = true;
            e.Handled = true;
        }

        //private void DefinitionContent_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)    // && e.OriginalSource is TextBox box
        //    {
        //        //box.IsSelectionActive = false;
        //        var poker = ((DefinitionEditorViewModel)DataContext).PokeCommand;
        //
        //        if (poker.CanExecute(null))
        //        {
        //            poker.Execute(null);
        //        }
        //        e.Handled = true;
        //    }
        //}
    }
}