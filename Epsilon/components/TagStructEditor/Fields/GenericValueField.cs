namespace TagStructEditor.Fields
{
    public abstract class GenericValueField<T> : ValueField
    {
        public T Value { get; set; }

        public GenericValueField(ValueFieldInfo info) : base(info)
        {
        }

        protected override void OnPopulate(object value)
        {
            Value = (T)value;
        }

        public virtual void OnValueChanged()
        {
            SetActualValue(Value);
        }

        public abstract override void Accept(IFieldVisitor visitor);
    }

    public class DebugValueField<T> : ValueField
    {
        public T Value { get; set; }

        public DebugValueField(ValueFieldInfo info) : base(info)
        {
        }

        protected override void OnPopulate(object value)
        {
            Value = (T)value;
        }

        public virtual void OnValueChanged()
        {
            SetActualValue(Value);
        }

        public override void Accept(IFieldVisitor visitor)
        {
            
        }
    }
}
