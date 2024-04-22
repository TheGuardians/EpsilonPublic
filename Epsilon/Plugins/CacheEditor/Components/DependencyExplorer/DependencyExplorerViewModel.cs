using EpsilonLib.Commands;
using EpsilonLib.Shell;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System.Linq;
using System.Windows.Input;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;

namespace CacheEditor.Components.DependencyExplorer
{
    class DependencyExplorerViewModel : CacheEditorTool
    {
        public const string ToolName = "CacheEditor.DependencyExplore";

        private ICacheEditor _editor;

        public DependencyExplorerViewModel(ICacheEditor editor)
        {
            _editor = editor;

            Name = ToolName;
            DisplayName = "Dependency Explorer";
            PreferredLocation = EpsilonLib.Shell.PaneLocation.Right;
            PreferredWidth = 450;
            IsVisible = true;

            var tagTree = (editor.TagTree as Components.TagTree.TagTreeViewModel);
            tagTree.NodeSelected += TagTree_NodeSelected;

            _editor.CurrentTagChanged += _editor_CurrentTagChanged;
        }

        public override bool InitialAutoHidden => true;

        public IObservableCollection<DependencyItem> Dependencies { get; } = new BindableCollection<DependencyItem>();
        public IObservableCollection<DependencyItem> Dependents { get; } = new BindableCollection<DependencyItem>();


        private void TagTree_NodeSelected(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Tag is CachedTagHaloOnline instance)
                Populate(instance);
        }

        internal void OnItemDoubleClicked(DependencyItem item)
        {
            _editor.OpenTag(item.Tag);
            if (item.Tag is CachedTagHaloOnline instance)
                Populate(instance);
        }

        private void _editor_CurrentTagChanged(object sender, System.EventArgs e)
        {
            Populate((CachedTagHaloOnline)_editor.CurrentTag);
        }

        private void Populate(CachedTagHaloOnline instance)
        {
            var cache = _editor.CacheFile.Cache;

            Dependencies.Clear();
            Dependents.Clear();

            if (instance == null)
                return;

            var dependencies = instance.Dependencies
                .Select(tagIndex => cache.TagCache.GetTag(tagIndex))
                .Select(CreateItem);

            var dependents = cache.TagCache.NonNull()
                .Cast<CachedTagHaloOnline>()
                .Where(tag => tag.Dependencies.Contains(instance.Index))
                .Select(CreateItem);
      
            Dependencies.AddRange(dependencies);
            Dependents.AddRange(dependents);
        }

        private DependencyItem CreateItem(CachedTag instance)
        {
            return new DependencyItem() { Tag = instance, DisplayName = $"{instance.Name}.{instance.Group.Tag}" };
        }

        public class DependencyItem
        {
            public CachedTag Tag { get; set; }
            public string DisplayName { get; set; }

            public ICommand CopyTagNameCommand { get; set; }
            public ICommand CopyTagIndexCommand { get; set; }

            public DependencyItem()
            {
                CopyTagNameCommand = new DelegateCommand(() => ClipboardEx.SetTextSafe($"{Tag}"));
                CopyTagIndexCommand = new DelegateCommand(() => ClipboardEx.SetTextSafe($"0x{Tag.Index:X08}"));
            }
        }
    }
}
