namespace TagStructEditor.Fields
{
    public interface IFieldVisitor
    {
        void Visit(StructField field);
        void Visit(RealVector3dField field);
        void Visit(InlineStructField field);
        void Visit(BlockField field);
        void Visit(CachedTagField field);
        void Visit(Point2dField field);
        void Visit(DatumHandleField field);
        void Visit(StringField field);
        void Visit(Rectangle2dField field);
        void Visit(ArgbColorField field);
        void Visit(RealRectangle3dField field);
        void Visit(RealRgbColorField field);
        void Visit(RealArgbColorField field);
        void Visit(RealRgbaColorField field);
        void Visit(StringIdField field);
        void Visit(RealVector2dField field);
        void Visit(RealEulerAngles2dField field);
        void Visit(DataField field);
        void Visit(BoolField field);
        void Visit(RealQuaternionField field);
        void Visit(AngleField field);
        void Visit(GroupTagField field);
        void Visit(CacheAddressField field);
        void Visit(RealEulerAngles3dField field);
        void Visit(Int8Field field);
        void Visit(RealPoint3dField field);
        void Visit(RealPlane2dField field);
        void Visit(RealPlane3dField field);
        void Visit(UInt8Field field);
        void Visit(FlagsField field);
        void Visit(EnumField field);
        void Visit(RealPoint2dField field);
        void Visit(Int16Field field);
        void Visit(UInt16Field field);
        void Visit(Int32Field field);
        void Visit(UInt32Field field);
        void Visit(Int64Field field);
        void Visit(UInt64Field field);
        void Visit(Float32Field field);
        void Visit(Float64Field field);
    }

    public abstract class FieldVisitorBase : IFieldVisitor
    {
        public virtual void Visit(RealVector3dField field) { }
        public virtual void Visit(CachedTagField field) { }
        public virtual void Visit(Point2dField field) { }
        public virtual void Visit(DatumHandleField field) { }
        public virtual void Visit(StringField field) { }
        public virtual void Visit(Rectangle2dField field) { }
        public virtual void Visit(ArgbColorField field) { }
        public virtual void Visit(RealRectangle3dField field) { }
        public virtual void Visit(RealRgbColorField field) { }
        public virtual void Visit(RealArgbColorField field) { }
        public virtual void Visit(RealRgbaColorField field) { }
        public virtual void Visit(StringIdField field) { }
        public virtual void Visit(RealVector2dField field) { }
        public virtual void Visit(RealEulerAngles2dField field) { }
        public virtual void Visit(DataField field) { }
        public virtual void Visit(BoolField field) { }
        public virtual void Visit(RealQuaternionField field) { }
        public virtual void Visit(AngleField field) { }
        public virtual void Visit(GroupTagField field) { }
        public virtual void Visit(CacheAddressField field) { }
        public virtual void Visit(RealEulerAngles3dField field) { }
        public virtual void Visit(Int8Field field) { }
        public virtual void Visit(RealPoint3dField field) { }
        public virtual void Visit(RealPlane2dField field) { }
        public virtual void Visit(RealPlane3dField field) { }
        public virtual void Visit(UInt8Field field) { }
        public virtual void Visit(FlagsField field) { }
        public virtual void Visit(EnumField field) { }
        public virtual void Visit(RealPoint2dField field) { }
        public virtual void Visit(Int16Field field) { }
        public virtual void Visit(UInt16Field field) { }
        public virtual void Visit(Int32Field field) { }
        public virtual void Visit(UInt32Field field) { }
        public virtual void Visit(Int64Field field) { }
        public virtual void Visit(UInt64Field field) { }
        public virtual void Visit(Float32Field field) { }
        public virtual void Visit(Float64Field field) { }

        public virtual void Visit(StructField field)
        {
            foreach (var child in field.Fields)
                child.Accept(this);
        }

        public virtual void Visit(InlineStructField field)
        {
            foreach (var child in field.Fields)
                child.Accept(this);
        }

        public virtual void Visit(BlockField field)
        {
            if (field.CurrentIndex != -1)
                field.Template.Accept(this);
        }
    }
}
