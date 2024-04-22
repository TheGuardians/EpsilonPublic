namespace CacheEditor.TagEditing.Messages
{
    public class DefinitionDataChangedEvent
    {
        public object NewData { get; }

        public DefinitionDataChangedEvent(object data)
        {
            NewData = data;
        }
    }
}
