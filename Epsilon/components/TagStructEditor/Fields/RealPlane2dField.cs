using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealPlane2dField : ValueField
    {
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }
        public float D { get; set; }

        public RealPlane2dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var plane = (RealPlane2d)value;
            I = plane.I;
            J = plane.J;
            D = plane.D;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealPlane2d(I, J, D));
        }

        public void OnIChanged() => UpdateValue();
        public void OnJChanged() => UpdateValue();
        public void OnDChanged() => UpdateValue();
    }
}
