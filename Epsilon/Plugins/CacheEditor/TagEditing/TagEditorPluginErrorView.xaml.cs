using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CacheEditor
{
    /// <summary>
    /// Interaction logic for TagEditorPluginErrorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TagEditorPluginErrorView : UserControl
    {
        public TagEditorPluginErrorView()
        {
            InitializeComponent();
        }
    }
}
