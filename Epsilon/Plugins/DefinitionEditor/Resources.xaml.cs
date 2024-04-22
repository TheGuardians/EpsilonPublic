using System.ComponentModel.Composition;
using System.Windows;

namespace TextEditor
{
    [Export(typeof(ResourceDictionary))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Resources : ResourceDictionary
    {
        public Resources()
        {
            InitializeComponent();
        }
    }
}
