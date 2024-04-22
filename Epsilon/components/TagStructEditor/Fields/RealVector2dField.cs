using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealVector2dField : ValueField
    {
        public float I { get; set; }
        public float J { get; set; }

        public RealVector2dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var vector = (RealVector2d)value;
            I = vector.I;
            J = vector.J;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealVector2d(I, J));
        }

        public void OnIChanged() => UpdateValue();
        public void OnJChanged() => UpdateValue();
    }
}
