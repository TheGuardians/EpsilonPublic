using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DefinitionEditor.Options
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
