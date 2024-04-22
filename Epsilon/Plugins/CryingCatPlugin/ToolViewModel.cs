using CacheEditor;
using EpsilonLib.Shell;
using System.ComponentModel.Composition;

namespace CryingCatPlugin
{
    class ToolViewModel : CacheEditorTool
    {
        public const string ToolName = "CryingCat.Tool";

        public ToolViewModel()
        {
            Name = ToolName;
            DisplayName = "Cryintg Cat";
            PreferredLocation = PaneLocation.Bottom;
            PreferredHeight = 400;
        }
    }

    [Export(typeof(ICacheEditorToolProvider))]
    class ToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => 1;

        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {
            return new ToolViewModel();
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return true;
        }
    }
}
