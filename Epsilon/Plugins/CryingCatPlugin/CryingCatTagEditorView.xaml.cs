using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CryingCatPlugin
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CryingCatTagEditorView : UserControl
    {
        public CryingCatTagEditorView()
        {
            InitializeComponent();
        }
    }
}
