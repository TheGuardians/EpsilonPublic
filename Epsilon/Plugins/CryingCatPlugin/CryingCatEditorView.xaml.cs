using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CryingCatPlugin
{
    /// <summary>
    /// Interaction logic for CryingCatEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CryingCatEditorView : UserControl
    {
        public CryingCatEditorView()
        {
            InitializeComponent();
        }
    }
}
