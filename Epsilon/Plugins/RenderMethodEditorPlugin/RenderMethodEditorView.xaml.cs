using System.Windows.Controls;
using System.ComponentModel.Composition;

namespace RenderMethodEditorPlugin
{
    /// <summary>
    /// Interaction logic for RenderMethodEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RenderMethodEditorView : UserControl
    {
        public RenderMethodEditorView()
        {
            InitializeComponent();
        }

        private void ListBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
