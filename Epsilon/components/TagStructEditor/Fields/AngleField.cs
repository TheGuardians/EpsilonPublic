using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class AngleField : ValueField
    {
        public float Value { get; set; }

        public AngleField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            Value = ((Angle)value).Degrees;
        }

        public void OnValueChanged() => SetActualValue(Angle.FromDegrees(Value));
    }
}
