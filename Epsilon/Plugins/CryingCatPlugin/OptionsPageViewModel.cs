using EpsilonLib.Options;
using Stylet;
using System.ComponentModel.Composition;

namespace CryingCatPlugin
{
    [Export(typeof(IOptionsPage))]
    class OptionsPageViewModel : Screen, IOptionsPage
    {
        public string Category => "Crying Cat";

        public bool IsDirty { get; set; }

        public OptionsPageViewModel()
        {
            DisplayName = "General";
        }

        public void Apply()
        {
            //throw new NotImplementedException();
        }

        public void Load()
        {
            //throw new NotImplementedException();
        }
    }
}
