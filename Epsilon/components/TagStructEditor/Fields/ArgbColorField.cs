using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class ArgbColorField : ValueField
    {
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public ArgbColorField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var color = (ArgbColor)value;
            Alpha = color.Alpha;
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
        }

        private void UpdateValue()
        {
            SetActualValue(new ArgbColor(Alpha, Red, Green, Blue));
        }

        public void OnAlphaChanged() => UpdateValue();
        public void OnRedChanged() => UpdateValue();
        public void OnGreenChanged() => UpdateValue();
        public void OnBlueChanged() => UpdateValue();
    }
}
