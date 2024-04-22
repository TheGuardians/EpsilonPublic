using System.ComponentModel.Composition;
using EpsilonLib.Controls;

namespace Epsilon.Options
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OptionsView : ChromeWindow
    {
        public OptionsView()
        {
            InitializeComponent();
        }
    }
}
