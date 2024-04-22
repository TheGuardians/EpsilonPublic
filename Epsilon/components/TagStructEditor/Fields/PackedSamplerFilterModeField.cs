using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags;
using static TagStructEditor.Fields.EnumField;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock.TextureConstant;

namespace TagStructEditor.Fields
{
    public class PackedSamplerFilterModeField : ValueField
    {
        public EnumMember ValueFilterMode { get; set; }
        public byte ValueAnisotropy { get; set; }
        public ObservableCollection<EnumMember> FilterModeValues { get; }

        public PackedSamplerFilterModeField(ValueFieldInfo info, CacheVersion version, CachePlatform platform) : base(info)
        {
            var enumInfo = TagEnum.GetInfo(typeof(SamplerFilterMode), version, platform);
            FilterModeValues = new ObservableCollection<EnumMember>(GenerateMemberList(enumInfo));
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var packedFieldMode = (PackedSamplerFilterMode)value;
            ValueFilterMode = FilterModeValues.FirstOrDefault(member => member.Value.Equals((Enum)packedFieldMode.FilterMode));
            ValueAnisotropy = packedFieldMode.Anisotropy;
        }

        protected void OnValueFilterModeChanged() => UpdateValue();
        protected void OnValueAnisotropyChanged() => UpdateValue();

        void UpdateValue()
        {
            SamplerFilterMode filterMode = ValueFilterMode == null ? SamplerFilterMode.Trilinear : (SamplerFilterMode)ValueFilterMode.Value;
            SetActualValue(new PackedSamplerFilterMode() { FilterMode = filterMode, Anisotropy = ValueAnisotropy });
        }
    }
}
