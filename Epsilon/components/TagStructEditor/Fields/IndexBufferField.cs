using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class IndexBufferIndexField : ValueField
    {
        public int Value { get; set; }

        public IndexBufferIndexField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var index = (IndexBufferIndex)value;
            Value = index;
        }

        protected void OnValueChanged() => UpdateValue();

        void UpdateValue() => SetActualValue(new IndexBufferIndex(Value));
    }
}
