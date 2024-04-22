using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Epsilon.Options
{
    /// <summary>
    /// Interaction logic for OptionsPageTreeView.xaml
    /// </summary>
    /// 
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OptionsPageTreeView : UserControl
    {
        public OptionsPageTreeView()
        {
            InitializeComponent();
        }

    }
}
