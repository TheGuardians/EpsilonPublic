using EpsilonLib.Menus;
using EpsilonLib.Utils;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TagStructEditor.Fields;

namespace TagResourceEditorPlugin
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TagResourceEditorView : UserControl
    {
        public TagResourceEditorView()
        {
            InitializeComponent();
        }

        private void structContainer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // If it's a textbox, just use the default context menu for now.
            if (VisualTreeHelpers.FindAncestors<TextBox>((Visual)e.OriginalSource).FirstOrDefault() != null)
                return;

            // Search the ancestors for the closest field
            var field = VisualTreeHelpers.FindAncestorDataContext<IField>((UIElement)e.OriginalSource);
            if (field == null)
                return;

            if (!(DataContext is TagResourceEditorViewModel vm))
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
    }
}
