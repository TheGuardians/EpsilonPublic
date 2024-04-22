using Stylet;
using System;
using System.Threading.Tasks;

namespace CacheEditor
{
    class TagEditorPluginTabViewModel : Screen
    {
        private IScreen _content;

        public Task<ITagEditorPlugin> LoadTask { get; }

        public IScreen Content
        {
            get => _content;
            set => SetAndNotify(ref _content, value);
        }

        public TagEditorPluginTabViewModel(Task<ITagEditorPlugin> futurePlugin)
        {
            LoadTask = futurePlugin;
            DoLoadingAsync(futurePlugin); 
        }

        private async void DoLoadingAsync(Task<ITagEditorPlugin> futurePlugin)
        {
            try
            {
                var plugin = await futurePlugin;
                Content = plugin;
            }
            catch(Exception ex)
            {
                Content = new TagEditorPluginErrorViewModel(ex);
            }
        }

        protected override void OnClose()
        {
            _content?.Close();
            _content = null;
        }
    }
}
