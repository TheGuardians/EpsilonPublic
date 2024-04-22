using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealMatrix4x3Field : InlineStructField
    {
        public RealMatrix4x3 Value;
        public RealVector3d Forward;
        public RealVector3d Left;
        public RealVector3d Up;
        public RealPoint3d Position;

        public RealMatrix4x3Field(ValueFieldInfo info) : base(info)
        {
            CreateChildren();
        }

        protected override void OnPopulate(object value)
        {
            var matrix = (RealMatrix4x3)value;
            Forward = new RealVector3d(matrix.m11, matrix.m12, matrix.m13);
            Left = new RealVector3d(matrix.m21, matrix.m22, matrix.m23);
            Up = new RealVector3d(matrix.m31, matrix.m32, matrix.m33);
            Position = new RealPoint3d(matrix.m41, matrix.m42, matrix.m43);

            base.OnPopulate(value);
        }

        private void CreateChildren()
        {
            var forwardField = new RealVector3dField(
                new ValueFieldInfo()
                {
                    FieldType = typeof(RealVector3d),
                    Name = "Forward",
                    ValueGetter = (owner) => Forward,
                    ValueSetter = (owner, value) => Forward = (RealVector3d)value,
                    ValueChanged = OnChange
                });

            var leftField = new RealVector3dField(
               new ValueFieldInfo()
               {
                   FieldType = typeof(RealVector3d),
                   Name = "Left",
                   ValueGetter = (owner) => Left,
                   ValueSetter = (owner, value) => Left = (RealVector3d)value,
                   ValueChanged = OnChange
               });

            var upField = new RealVector3dField(
                new ValueFieldInfo()
                {
                    FieldType = typeof(RealVector3d),
                    Name = "Up",
                    ValueGetter = (owner) => Up,
                    ValueSetter = (owner, value) => Up = (RealVector3d)value,
                    ValueChanged = OnChange
                });

            var positionField = new RealPoint3dField(
                new ValueFieldInfo()
                {
                    FieldType = typeof(RealPoint3d),
                    Name = "Position",
                    ValueGetter = (owner) => Position,
                    ValueSetter = (owner, value) => Position = (RealPoint3d)value,
                    ValueChanged = OnChange
                });

            AddChild(forwardField);
            AddChild(leftField);
            AddChild(upField);
            AddChild(positionField);
        }

        private void OnChange(ValueChangedEventArgs info)
        {
            SetActualValue(new RealMatrix4x3(
                Forward.I, Forward.J, Forward.K,
                Left.I, Left.J, Left.K,
                Up.I, Up.J, Up.K,
                Position.X, Position.Y, Position.Z));
        }
    }
}
