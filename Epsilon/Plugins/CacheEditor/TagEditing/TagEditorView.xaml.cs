using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace CacheEditor
{
    /// <summary>
    /// Interaction logic for TagEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TagEditorView : UserControl
    {
        public TagEditorView()
        {
            InitializeComponent();
            this.Unloaded += TagEditorView_Unloaded;
        }

        private void TagEditorView_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
