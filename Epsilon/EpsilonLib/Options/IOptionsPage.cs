using Stylet;

namespace EpsilonLib.Options
{
    public interface IOptionsPage : IScreen
    {
        string Category { get; }

        bool IsDirty { get; set; }

        void Apply();
    }

    public class ProvideOptionsPageAttribute
    {
        public string Category { get; set; }
    }
}
