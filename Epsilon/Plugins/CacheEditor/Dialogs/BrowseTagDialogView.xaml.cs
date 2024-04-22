using System.ComponentModel.Composition;
using EpsilonLib.Controls;
using Stylet;

namespace CacheEditor
{
    /// <summary>
    /// Interaction logic for BrowseTagDialog.xaml
    /// </summary> 
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class BrowseTagDialogView : ChromeWindow
    {
        public BrowseTagDialogView()
        {
            InitializeComponent();
        }
    }
}
