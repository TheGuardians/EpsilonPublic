using System;

namespace TagStructEditor.Fields
{
    public class StringField : ValueField
    {
        public int MaxLength { get; set; } = int.MaxValue;
        public string Value { get; set; }

        public StringField(ValueFieldInfo info) : base(info)
        {
            MaxLength = info.Length;
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            if (Value == null)
                Value = string.Empty;

            Value = (string)value;
        }

        public void OnValueChanged()
        {
            if (Value == null)
                return;

            if (Value.Length > MaxLength)
                throw new ArgumentException(nameof(Value), $"Length ({Value.Length}) exceeded MaxLength ({MaxLength})");

            SetActualValue(Value);
        }
    }
}
