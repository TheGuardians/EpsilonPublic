using System;

namespace TagStructEditor.Fields
{
    public interface IFieldFactory
    {
        StructField CreateStruct(Type structType);
        ValueField CreateValueField(ValueFieldInfo info);
    }
}
