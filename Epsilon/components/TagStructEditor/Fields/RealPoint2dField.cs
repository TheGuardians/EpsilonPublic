using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealPoint2dField : ValueField
    {
        public float X { get; set; }
        public float Y { get; set; }

        public RealPoint2dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var point = (RealPoint2d)value;
            X = point.X;
            Y = point.Y;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealPoint2d(X, Y));
        }

        public void OnXChanged() => UpdateValue();
        public void OnYChanged() => UpdateValue();
    }
}
