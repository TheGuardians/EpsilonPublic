using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealVector3dField : ValueField
    {
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }

        public RealVector3dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var vector = (RealVector3d)value;
            I = vector.I;
            J = vector.J;
            K = vector.K;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealVector3d(I, J, K));
        }

        public void OnIChanged() => UpdateValue();
        public void OnJChanged() => UpdateValue();
        public void OnKChanged() => UpdateValue();
    }
}
