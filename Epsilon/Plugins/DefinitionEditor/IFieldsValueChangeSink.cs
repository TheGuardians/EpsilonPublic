using System;
using TagStructEditor.Fields;

namespace DefinitionEditor
{
    public interface IFieldsValueChangeSink
    {
        event EventHandler<ValueChangedEventArgs> ValueChanged;
    }
}
