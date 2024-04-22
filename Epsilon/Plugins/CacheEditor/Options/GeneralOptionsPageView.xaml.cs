using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CacheEditor.Options
{
    /// <summary>
    /// Interaction logic for CacheEditorOptionsPageView.xaml
    /// </summary>

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class GeneralOptionsPageView : UserControl
    {
        public GeneralOptionsPageView()
        {
            InitializeComponent();
        }
    }
}
