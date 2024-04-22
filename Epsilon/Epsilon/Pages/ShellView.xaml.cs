using System.ComponentModel.Composition;
using System.Windows;
using EpsilonLib.Controls;

namespace Epsilon.Pages
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    [Export]

    public partial class ShellView : ChromeWindow
    {
        public ShellView()
        {
            InitializeComponent();

        }

        private void Shell_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 0)
                return;

            if (DataContext is ShellViewModel vm)
            {
                vm.OnDroppedFiles(files);
            }
        }
    }
}
