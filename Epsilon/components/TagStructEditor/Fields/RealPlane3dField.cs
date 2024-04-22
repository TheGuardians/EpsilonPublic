using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealPlane3dField : ValueField
    {
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }
        public float D { get; set; }

        public RealPlane3dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var plane = (RealPlane3d)value;
            I = plane.I;
            J = plane.J;
            K = plane.K;
            D = plane.D;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealPlane3d(I, J, K, D));
        }

        public void OnIChanged() => UpdateValue();
        public void OnJChanged() => UpdateValue();
        public void OnKChanged() => UpdateValue();
        public void OnDChanged() => UpdateValue();
    }
}
