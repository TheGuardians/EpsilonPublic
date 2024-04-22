using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealRectangle3dField : ValueField
    {
        public float X0 { get; set; }
        public float X1 { get; set; }
        public float Y0 { get; set; }
        public float Y1 { get; set; }
        public float Z0 { get; set; }
        public float Z1 { get; set; }

        public RealRectangle3dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var rectangle = (RealRectangle3d)value;
            X0 = rectangle.X0;
            X1 = rectangle.X1;
            Y0 = rectangle.Y0;
            Y1 = rectangle.Y1;
            Z0 = rectangle.Z0;
            Z1 = rectangle.Z1;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealRectangle3d(X0, X1, Y0, Y1, Z0, Z1));
        }

        public void OnX0Changed() => UpdateValue();
        public void OnX1Changed() => UpdateValue();
        public void OnY0Changed() => UpdateValue();
        public void OnY1Changed() => UpdateValue();
        public void OnZ0Changed() => UpdateValue();
        public void OnZ1Changed() => UpdateValue();
    }
}
