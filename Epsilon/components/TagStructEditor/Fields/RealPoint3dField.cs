using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealPoint3dField : ValueField
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public RealPoint3dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var point = (RealPoint3d)value;
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealPoint3d(X, Y, Z));
        }

        public void OnXChanged() => UpdateValue();
        public void OnYChanged() => UpdateValue();
        public void OnZChanged() => UpdateValue();
    }
}
