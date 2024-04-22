using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealEulerAngles2dField : ValueField
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public RealEulerAngles2dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var angles = (RealEulerAngles2d)value;
            Yaw = angles.Yaw.Degrees;
            Pitch = angles.Pitch.Degrees;
        }

        private void UpdateValue()
        {
            SetActualValue(
                new RealEulerAngles2d(
                    Angle.FromDegrees(Yaw),
                    Angle.FromDegrees(Pitch)));
        }

        public void OnYawChanged() => UpdateValue();
        public void OnPitchChanged() => UpdateValue();
    }
}
