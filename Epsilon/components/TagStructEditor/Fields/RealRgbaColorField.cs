using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealRgbaColorField : ValueField
    {
        public float Alpha { get; set; }
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealRgbaColorField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var color = (RealRgbaColor)value;
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
            Alpha = color.Alpha;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealRgbaColor(Red, Green, Blue, Alpha));
        }

        public void OnAlphaChanged() => UpdateValue();
        public void OnRedChanged() => UpdateValue();
        public void OnGreenChanged() => UpdateValue();
        public void OnBlueChanged() => UpdateValue();
    }
}
