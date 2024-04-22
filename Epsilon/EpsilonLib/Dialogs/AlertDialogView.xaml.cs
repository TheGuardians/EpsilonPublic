using System.ComponentModel.Composition;
using EpsilonLib.Controls;

namespace EpsilonLib.Dialogs
{
    /// <summary>
    /// Interaction logic for RenameTagDialogView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlertDialogView : ChromeWindow
    {
        public AlertDialogView()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            
        }
    }
}
