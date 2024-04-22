using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace MapVariantFixer
{
    /// <summary>
    /// Interaction logic for MapVariantFixerView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MapVariantFixerView : UserControl
    {
        public MapVariantFixerView()
        {
            InitializeComponent();
            this.DataContextChanged += MapVariantFixerView_DataContextChanged;
        }

        private void MapVariantFixerView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MapVariantFixerViewModel oldModel)
                oldModel.PropertyChanged -= Model_PropertyChanged;

            if (e.NewValue is MapVariantFixerViewModel newModel)
                newModel.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(MapVariantFixerViewModel.Output))
            {
                OutputTextBox.Select(OutputTextBox.Text.Length, 0);
                OutputTextBox.ScrollToEnd();
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                (this.DataContext as MapVariantFixerViewModel).AddFiles(files);
            }

            e.Handled = true;
        }
    }
}
