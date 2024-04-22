using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CryingCatPlugin
{
    /// <summary>
    /// Interaction logic for OptionsPageView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OptionsPageView : UserControl
    {
        public OptionsPageView()
        {
            InitializeComponent();
        }
    }
}
