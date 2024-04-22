using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ModPackagePlugin
{
    /// <summary>
    /// Interaction logic for MetadataEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MetadataEditorView : UserControl
    {
        public MetadataEditorView()
        {
            InitializeComponent();
        }
    }
}
