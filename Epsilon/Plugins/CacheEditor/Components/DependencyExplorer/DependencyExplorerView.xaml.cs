using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using static CacheEditor.Components.DependencyExplorer.DependencyExplorerViewModel;

namespace CacheEditor.Components.DependencyExplorer
{
    /// <summary>
    /// Interaction logic for DependencyExplorerView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DependencyExplorerView : UserControl
    {
        public DependencyExplorerView()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListBoxItem).DataContext as DependencyItem;
            if (DataContext is DependencyExplorerViewModel model)
                model.OnItemDoubleClicked(item);
        }
    }
}
