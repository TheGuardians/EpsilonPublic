using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CacheEditor
{
    /// <summary>
    /// Interaction logic for MyToolView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TagExplorerView : UserControl
    {
        public TagExplorerView()
        {
            InitializeComponent();
        }
    }
}
