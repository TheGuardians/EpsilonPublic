using Stylet;

namespace CacheEditor.TagEditing
{
    public abstract class TagEditorPluginBase : Screen, ITagEditorPlugin
    {
        private ITagEditorPluginClient _client;

        ITagEditorPluginClient ITagEditorPlugin.Client
        {
            set => _client = value;
        }

        void ITagEditorPlugin.OnMessage(object sender, object message)
        {
            OnMessage(sender, message);
        }

        protected virtual void OnMessage(object sender, object message) { }

        protected virtual void PostMessage(object sender, object message)
        {
            _client.PostMessage(sender, message);
        }
    }
}
