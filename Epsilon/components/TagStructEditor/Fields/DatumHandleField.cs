using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class DatumHandleField : ValueField
    {
        public ushort Salt { get; set; }
        public ushort Index { get; set; }

        public DatumHandleField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var handle = (DatumHandle)value;
            Salt = handle.Salt;
            Index = handle.Index;
        }

        private void UpdateValue()
        {
            SetActualValue(new DatumHandle(Salt, Index));
        }

        public void OnSaltChanged() => UpdateValue();
        public void OnIndexChanged() => UpdateValue();
    }
}
