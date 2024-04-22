using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CacheEditor.Components.Info
{
    /// <summary>
    /// Interaction logic for CacheInfoToolView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CacheInfoToolView : UserControl
    {
        public CacheInfoToolView()
        {
            InitializeComponent();
        }
    }
}
