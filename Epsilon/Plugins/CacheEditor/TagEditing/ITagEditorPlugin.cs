using Stylet;

namespace CacheEditor
{
    public interface ITagEditorPlugin : IScreen
    {
        ITagEditorPluginClient Client { set; }
        void OnMessage(object sender, object message);
    }

    public interface ITagEditorPluginClient
    {
        void PostMessage(object sender, object message);
    }
}
