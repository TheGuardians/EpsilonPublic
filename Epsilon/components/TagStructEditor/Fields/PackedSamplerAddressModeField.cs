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
    public class PackedSamplerAddressModeField : ValueField
    {
        public EnumMember ValueU { get; set; }
        public EnumMember ValueV { get; set; }
        public ObservableCollection<EnumMember> Values { get; }

        public PackedSamplerAddressModeField(ValueFieldInfo info, CacheVersion version, CachePlatform platform) : base(info)
        {
            var enumInfo = TagEnum.GetInfo(typeof(SamplerAddressModeEnum), version, platform);
            Values = new ObservableCollection<EnumMember>(GenerateMemberList(enumInfo));
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var packedAddressMode = (PackedSamplerAddressMode)value;
            ValueU = Values.FirstOrDefault(member => member.Value.Equals((Enum)packedAddressMode.AddressU));
            ValueV = Values.FirstOrDefault(member => member.Value.Equals((Enum)packedAddressMode.AddressV));
        }

        protected void OnValueUChanged() => UpdateValue();
        protected void OnValueVChanged() => UpdateValue();

        void UpdateValue()
        {
            SamplerAddressModeEnum addressU = ValueU == null ? SamplerAddressModeEnum.Wrap : (SamplerAddressModeEnum)ValueU.Value;
            SamplerAddressModeEnum addressV = ValueV == null ? SamplerAddressModeEnum.Wrap : (SamplerAddressModeEnum)ValueV.Value;
            SetActualValue(new PackedSamplerAddressMode() { AddressU = addressU, AddressV = addressV });
        }
    }
}
