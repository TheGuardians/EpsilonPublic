using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealRgbColorField : ValueField
    {
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealRgbColorField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var color = (RealRgbColor)value;
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealRgbColor(Red, Green, Blue));
        }

        public void OnRedChanged() => UpdateValue();
        public void OnGreenChanged() => UpdateValue();
        public void OnBlueChanged() => UpdateValue();
    }
}
