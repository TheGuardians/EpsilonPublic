namespace TagStructEditor.Fields
{
    public class BoolField : GenericValueField<bool>
    {
        public BoolField(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Int8Field : GenericValueField<sbyte>
    {
        public Int8Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class UInt8Field : GenericValueField<byte>
    {
        public UInt8Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Int16Field : GenericValueField<short>
    {
        public Int16Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class UInt16Field : GenericValueField<ushort>
    {
        public UInt16Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Int32Field : GenericValueField<int>
    {
        public Int32Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class UInt32Field : GenericValueField<uint>
    {
        public UInt32Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Int64Field : GenericValueField<long>
    {
        public Int64Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class UInt64Field : GenericValueField<ulong>
    {
        public UInt64Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Float32Field : GenericValueField<float>
    {
        public Float32Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }

    public class Float64Field : GenericValueField<double>
    {
        public Float64Field(ValueFieldInfo info) : base(info) { }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);
    }
}
