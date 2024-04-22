using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class Rectangle2dField : ValueField
    {
        public short Top { get; set; }
        public short Left { get; set; }
        public short Bottom { get; set; }
        public short Right { get; set; }

        public Rectangle2dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var rectangle = (Rectangle2d)value;
            Top = rectangle.Top;
            Left = rectangle.Left;
            Bottom = rectangle.Bottom;
            Right = rectangle.Right;
        }

        private void UpdateValue()
        {
            SetActualValue(new Rectangle2d(Top, Left, Bottom, Right));
        }

        public void OnTopChanged() => UpdateValue();
        public void OnLeftChanged() => UpdateValue();
        public void OnBottomChanged() => UpdateValue();
        public void OnRightChanged() => UpdateValue();
    }
}
