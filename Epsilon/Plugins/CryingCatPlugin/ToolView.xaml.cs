using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CryingCatPlugin
{
    /// <summary>
    /// Interaction logic for ToolView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ToolView : UserControl
    {
        public ToolView()
        {
            InitializeComponent();
        }
    }
}
