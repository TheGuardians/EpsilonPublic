using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealArgbColorField : ValueField
    {
        public float Alpha { get; set; }
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealArgbColorField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var color = (RealArgbColor)value;
            Alpha = color.Alpha;
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
        }

        private void UpdateValue()
        {
            SetActualValue(new RealArgbColor(Alpha, Red, Green, Blue));
        }

        public void OnAlphaChanged() => UpdateValue();
        public void OnRedChanged() => UpdateValue();
        public void OnGreenChanged() => UpdateValue();
        public void OnBlueChanged() => UpdateValue();
    }
}
