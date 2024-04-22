using System;
using TagTool.Tags;
using static TagTool.Tags.TagFieldInfo;

namespace TagStructEditor.Fields
{
    /// <summary>
    /// Info used to construct a <see cref="ValueField"/>
    /// </summary>
    public class ValueFieldInfo : IDisposable
    {
        public ValueFieldFlags Flags { get; set; } = ValueFieldFlags.Default;
        public Type FieldType { get; set; }
        public string ActualName { get; set; }
        public string Name { get; set; }
        public uint Offset { get; set; }
        public int Length { get; set; }
        public ValueSetter ValueSetter;
        public ValueGetter ValueGetter;
        public Action<ValueChangedEventArgs> ValueChanged;
        public TagFieldAttribute Attribute;

        public void Dispose()
        {
            ValueChanged = null;
            ValueGetter = null;
            ValueSetter = null;
            Attribute = null;
        }
    }

    [Flags]
    public enum ValueFieldFlags
    {
        None = 0,
        ShowType = (1 << 0),

        Default = ShowType
    }
}
