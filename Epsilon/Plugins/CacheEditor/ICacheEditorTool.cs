using EpsilonLib.Shell;
using Shared;
using Stylet;

namespace CacheEditor
{
    public interface ICacheEditorTool : ITool, IScreen
    {
        bool InitialAutoHidden { get; }
        void Show(bool show, bool activate = false);
    }

    public class CacheEditorTool : Screen, ICacheEditorTool
    {
        private bool _isVisible = false;
        private bool _isActive = false;

        public string Name { get; set; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetAndNotify(ref _isVisible, value);
        }

        public PaneLocation PreferredLocation { get; set; }

        public int PreferredWidth { get; set; }

        public int PreferredHeight { get; set; }
         
        public new bool IsActive
        {
            get => _isActive;
            set => SetAndNotify(ref _isActive, value);
        }


        public virtual bool InitialAutoHidden => false;

        public void Show(bool show, bool activate)
        {
            IsVisible = show;
            IsActive = show && activate;
        }
    }

    public interface ICacheEditorToolProvider
    {
        int SortOrder { get; }

        bool ValidForEditor(ICacheEditor editor);
        ICacheEditorTool CreateTool(ICacheEditor editor);
    }
}
