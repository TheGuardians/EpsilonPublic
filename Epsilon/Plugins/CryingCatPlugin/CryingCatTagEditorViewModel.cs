using CacheEditor;
using CacheEditor.TagEditing;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using TagTool.Cache;

namespace CryingCatPlugin
{
    class CryingCatTagEditorViewModel : TagEditorPluginBase
    {
        private CachedTag _tag;
        public CryingCatTagEditorViewModel(TagEditorContext context)
        {
            _tag = context.Instance;
        }

        public void SaveCat()
        {
            MessageBox.Show($"The cat has been saved! also the tag being edited is if you care: {_tag}");
        }
    }

    [Export(typeof(ITagEditorPluginProvider))]
    class CryingCatTagPluginProvider : ITagEditorPluginProvider
    {
        public string DisplayName => "Cat Editor";

        public int SortOrder => 1;

        public Task<ITagEditorPlugin> CreateAsync(TagEditorContext context)
        {
            return Task.FromResult<ITagEditorPlugin>(new CryingCatTagEditorViewModel(context));
        }

        public bool ValidForTag(ICacheFile cache, CachedTag tag)
        {
            return true;
        }
    }
}
