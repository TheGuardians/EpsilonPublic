using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealEulerAngles3dField : ValueField
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public RealEulerAngles3dField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var angles = (RealEulerAngles3d)value;
            Yaw = angles.Yaw.Degrees;
            Pitch = angles.Pitch.Degrees;
            Roll = angles.Roll.Degrees;
        }

        private void UpdateValue()
        {
            SetActualValue(
                new RealEulerAngles3d(
                    Angle.FromDegrees(Yaw),
                    Angle.FromDegrees(Pitch),
                    Angle.FromDegrees(Roll)));
        }

        public void OnYawChanged() => UpdateValue();
        public void OnPitchChanged() => UpdateValue();
        public void OnRollChanged() => UpdateValue();
    }
}
