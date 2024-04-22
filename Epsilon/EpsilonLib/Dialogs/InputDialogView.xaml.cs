using System.ComponentModel.Composition;
using EpsilonLib.Controls;

namespace EpsilonLib.Dialogs
{
    /// <summary>
    /// Interaction logic for RenameTagDialogView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class InputDialogView : ChromeWindow
    {
        public InputDialogView()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            InputTextBox.Focus();
            InputTextBox.CaretIndex = InputTextBox.Text.Length;
        }

        private void InputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true;
                //ConfirmButton_Click();
            }
        }

    }
}
