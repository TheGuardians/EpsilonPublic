using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagStructEditor.Common;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Shaders;
using TagTool.Tags;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock.TextureConstant;
using static TagTool.Tags.Definitions.RenderMethodTemplate;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData;

namespace TagStructEditor.Fields
{
    public class FieldFactory : IFieldFactory
    {
        private readonly Configuration _config;
        private readonly GameCache _cache;
        private readonly TagList _tagList;

        public FieldFactory(GameCache cache, TagList tagList, Configuration config)
        {
            _cache = cache;
            _tagList = tagList;
            _config = config;
        }

        public StructField CreateStruct(Type structType)
        {
            var newField = new StructField();
            foreach (var child in CreateStructChildren(structType))
                newField.AddChild(child);

            return newField;
        }

        private InlineStructField CreateInlineStruct(ValueFieldInfo info)
        {
            var newField = new InlineStructField(info);
            foreach (var child in CreateStructChildren(info.FieldType))
                newField.AddChild(child);

            return newField;
        }

        private IEnumerable<IField> CreateStructChildren(Type structType)
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(structType, _cache.Version, _cache.Platform))
            {
                var attribute = tagFieldInfo.Attribute;

                if (attribute != null)
                {
                    if (attribute.Flags.HasFlag(TagFieldFlags.Padding)
                        || attribute.Flags.HasFlag(TagFieldFlags.Runtime)
                        || attribute.Flags.HasFlag(TagFieldFlags.Hidden))
                        continue;
                }

                var name = tagFieldInfo.Name;
                //if (_config.DisplayFieldOffsets)
                //    name = $"[ 0x{tagFieldInfo.Offset:x} ] {name}";

                var info = new ValueFieldInfo()
                {
                    Name = name,
                    ActualName = tagFieldInfo.Name,
                    FieldType = tagFieldInfo.FieldType,
                    ValueGetter = tagFieldInfo.GetValue,
                    ValueSetter = tagFieldInfo.SetValue,
                    Offset = tagFieldInfo.Offset,
                    ValueChanged = _config.ValueChanged,
                    Attribute = attribute,
                    Length = attribute?.Length ?? 0
                };

                yield return CreateValueField(info);
            }
        }

        private BlockField CreateBlockField(ValueFieldInfo info)
        {
            var elementType = info.FieldType.IsArray ?
                info.FieldType.GetElementType() :
                info.FieldType.GetGenericArguments()[0];

            var blockField = new BlockField(elementType, info);

            if (info.Length > 0)
                blockField.IsFixedSize = true;

            var template = CreateBlockTemplateField(elementType, blockField);
            template.Parent = blockField;
            blockField.Template = template;

            if (_config.CollapseBlocks)
                blockField.IsExpanded = false;

            return blockField;
        }

        private IField CreateBlockTemplateField(Type elementType, BlockField blockField)
        {
            
            var info = new ValueFieldInfo()
            {
                Name = "[Value]",
                FieldType = elementType,
                ValueGetter = (owner) => blockField.Block[blockField.CurrentIndex],
                ValueSetter = (owner, value) => blockField.Block[blockField.CurrentIndex] = value,
                ValueChanged = _config.ValueChanged
            };

            if (elementType == typeof(TagBlockIndex))
                return CreateValueField(info);
            else if (elementType == typeof(Bsp3dNode))
                return new Bsp3dNodeField(info);

            if (typeof(TagStructure).IsAssignableFrom(elementType))
                return CreateStruct(elementType);

            // Handle the lists of value types case

            return CreateValueField(info);
        }

        public ValueField CreateValueField(ValueFieldInfo info)
        {
            if (_config.DisplayFieldTypes)
                info.Flags |= ValueFieldFlags.ShowType;
            else
                info.Flags &= ~ValueFieldFlags.ShowType;

            if (typeof(IList).IsAssignableFrom(info.FieldType))
            {
                Type elementType;
                if (info.FieldType.IsArray)
                    elementType = info.FieldType.GetElementType();
                else
                    elementType = info.FieldType.GetGenericArguments()[0];

                if (info.FieldType.IsArray && elementType == typeof(byte))
                    return new DataField(info);

                return CreateBlockField(info);
            }
            else if (info.FieldType == typeof(TagData))
                return new TagDataField(info);
            else if (info.FieldType == typeof(TagBlockIndex))
                return new TagBlockIndexField(info);
            else if (info.FieldType == typeof(Property.InnardsY))
                return new EmitterGpuInnardsYField(info);
            else if (info.FieldType == typeof(Property.InnardsZ))
                return new EmitterGpuInnardsZField(info);
            else if (info.FieldType == typeof(Property.InnardsW))
                return new EmitterGpuInnardsWField(info);
            else if (info.FieldType == typeof(Function.FunctionTypeReal))
                return new FunctionTypeRealField(info);
            else if (info.FieldType == typeof(PackedSamplerAddressMode))
                return new PackedSamplerAddressModeField(info, _cache.Version, _cache.Platform);
            else if (info.FieldType == typeof(PackedSamplerFilterMode))
                return new PackedSamplerFilterModeField(info, _cache.Version, _cache.Platform);
            else if (info.FieldType == typeof(Bsp3dNode))
                return new Bsp3dNodeField(info);
            else if (info.FieldType == typeof(TagFunction))
                return new TagFunctionField(info);
            else if (typeof(TagStructure).IsAssignableFrom(info.FieldType))
                return CreateInlineStruct(info);
            else if (typeof(IBounds).IsAssignableFrom(info.FieldType))
                return new BoundsField(this, info);
            else if (info.FieldType.IsEnum && !info.FieldType.GetCustomAttributes(typeof(FlagsAttribute)).Any())
                return new EnumField(info, TagEnum.GetInfo(info.FieldType, _cache.Version, _cache.Platform));
            else if (info.FieldType.IsEnum)
                return new FlagsField(info, _cache.Version, _cache.Platform);
            else if (info.FieldType == typeof(CachedTag))
                return new CachedTagField(info, _tagList, _config.OpenTag, _config.BrowseTag);
            else if (info.FieldType == typeof(RealMatrix4x3))
                return new RealMatrix4x3Field(info);
            else if (info.FieldType == typeof(RealPoint3d))
                return new RealPoint3dField(info);
            else if (info.FieldType == typeof(RealPoint2d))
                return new RealPoint2dField(info);
            else if (info.FieldType == typeof(RealVector3d))
                return new RealVector3dField(info);
            else if (info.FieldType == typeof(RealVector2d))
                return new RealVector2dField(info);
            else if (info.FieldType == typeof(RealEulerAngles2d))
                return new RealEulerAngles2dField(info);
            else if (info.FieldType == typeof(RealEulerAngles3d))
                return new RealEulerAngles3dField(info);
            else if (info.FieldType == typeof(Angle))
                return new AngleField(info);
            else if (info.FieldType == typeof(RealQuaternion))
                return new RealQuaternionField(info);
            else if (info.FieldType == typeof(RealPlane3d))
                return new RealPlane3dField(info);
            else if (info.FieldType == typeof(StringId))
                return new StringIdField(_cache.StringTable, info);
            else if (info.FieldType == typeof(RealArgbColor))
                return new RealArgbColorField(info);
            else if (info.FieldType == typeof(RealRgbaColor))
                return new RealRgbaColorField(info);
            else if (info.FieldType == typeof(RealRgbColor))
                return new RealRgbColorField(info);
            else if (info.FieldType == typeof(DatumHandle))
                return new DatumHandleField(info);
            else if (info.FieldType == typeof(Point2d))
                return new Point2dField(info);
            else if (info.FieldType == typeof(ArgbColor))
                return new ArgbColorField(info);
            else if (info.FieldType == typeof(CacheAddress))
                return new CacheAddressField(info);
            else if (info.FieldType == typeof(Tag))
                return new GroupTagField(_tagList, info);
            else if (info.FieldType == typeof(RealPlane2d))
                return new RealPlane2dField(info);
            else if (info.FieldType == typeof(RealRectangle3d))
                return new RealRectangle3dField(info);
            else if (info.FieldType == typeof(Rectangle2d))
                return new Rectangle2dField(info);
            else if (info.FieldType == typeof(VertexShaderReference))
                return new InlineStructField(info);
            else if (info.FieldType == typeof(PixelShaderReference))
                return new InlineStructField(info);
            else if (info.FieldType == typeof(IndexBufferIndex))
                return new IndexBufferIndexField(info);
            else if (info.FieldType == typeof(StructureSurfaceToTriangleMapping))
                return new PlaneReferenceField(info);
            else if (typeof(IFlagBits).IsAssignableFrom(info.FieldType))
                return new FlagBitsField(info, _cache.Version, _cache.Platform);
           

            switch (Type.GetTypeCode(info.FieldType))
            {
                case TypeCode.String:
                    return new StringField(info);
                case TypeCode.Boolean:
                    return new BoolField(info);
                case TypeCode.SByte:
                    return new Int8Field(info);
                case TypeCode.Byte:
                    return new UInt8Field(info);
                case TypeCode.Int16:
                    return new Int16Field(info);
                case TypeCode.UInt16:
                    return new UInt16Field(info);
                case TypeCode.Int32:
                    return new Int32Field(info);
                case TypeCode.UInt32:
                    return new UInt32Field(info);
                case TypeCode.Int64:
                    return new Int64Field(info);
                case TypeCode.UInt64:
                    return new UInt64Field(info);
                case TypeCode.Single:
                    return new Float32Field(info);
                case TypeCode.Double:
                    return new Float64Field(info);
            }

            return (ValueField)Activator.CreateInstance(typeof(DebugValueField<>).MakeGenericType(info.FieldType), info);

            throw new NotSupportedException($"Unknown field type: {info.FieldType}");
        }
    }
}
