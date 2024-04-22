namespace TagStructEditor.Fields
{
    public class BoundsField : InlineStructField
    {
        public BoundsField(IFieldFactory factory, ValueFieldInfo info) : base(info)
        {
            CreateChildren(factory);
        }

        private void CreateChildren(IFieldFactory factory)
        {
            var valueType = FieldType.GetGenericArguments()[0];

            var lowerProperty = FieldType.GetProperty("Lower");
            var upperProperty = FieldType.GetProperty("Upper");

            ValueField lower = factory.CreateValueField(
                new ValueFieldInfo()
                {
                    Name = lowerProperty.Name,
                    FieldType = valueType,
                    Offset = FieldOffset,
                    ValueGetter = lowerProperty.GetValue,
                    ValueSetter = lowerProperty.SetValue,
                    ValueChanged = OnChange
                });

            var upper = factory.CreateValueField(
                new ValueFieldInfo()
                {
                    Name = upperProperty.Name,
                    FieldType = valueType,
                    Offset = FieldOffset,
                    ValueGetter = upperProperty.GetValue,
                    ValueSetter = upperProperty.SetValue,
                    ValueChanged = OnChange
                });

            AddChild(lower);
            AddChild(upper);
        }

        private void OnChange(ValueChangedEventArgs info)
        {
            SetActualValue(info.Field.Owner);
        }
    }
}
