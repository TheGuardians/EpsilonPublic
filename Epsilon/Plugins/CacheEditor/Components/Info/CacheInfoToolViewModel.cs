using EpsilonLib.Shell;

namespace CacheEditor.Components.Info
{
    class CacheInfoToolViewModel : CacheEditorTool
    {
        public const string ToolName = "CacheEditor.InfoTool";

        public CacheInfoToolViewModel(ICacheEditor editor)
        {
            Name = ToolName;
            DisplayName = "Info";
            PreferredLocation = PaneLocation.Left;
            PreferredWidth = 400;
            IsVisible = true;

            Info = new CacheInfo(editor.CacheFile);
        }

        public CacheInfo Info { get; private set; }
    }
}
