using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TagStructEditor.Common;
using TagStructEditor.Helpers;
using TagTool.Cache;
using TagTool.Tags;

namespace TagStructEditor.Fields
{
    public class FlagsField : ValueField
    {
        public FlagsField(ValueFieldInfo info, CacheVersion version, CachePlatform platform) : base(info)
        {
            var enumInfo = TagEnum.GetInfo(info.FieldType, version, platform);
            EnumType = enumInfo.Type;

            Flags = new ObservableCollection<Flag>(GenerateFlagList(enumInfo));
        }

        public Type EnumType { get; }
        public ObservableCollection<Flag> Flags { get; }

        private IEnumerable<Flag> GenerateFlagList(TagEnumInfo enumInfo)
        {
            var members = TagEnum.GetMemberEnumerable(enumInfo).Members;
            for (int i = 0; i < members.Count; i++)
            {
                var value = (Enum)members[i].Value;
                dynamic flagValue = value;
                if (flagValue == 0)
                    continue;

                var name = Utils.DemangleName(members[i].Name);
                yield return new Flag(name, value, OnFlagToggled);
            }
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var valueEnum = (Enum)value;

            foreach (var member in Flags)
                member.IsChecked = valueEnum.HasFlag(member.Value);
        }

        private void OnFlagToggled()
        {
            if(!IsPopulating)
                SetActualValue(ComputeValue());
        }

        private object ComputeValue()
        {
            switch (Type.GetTypeCode(EnumType))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return Enum.ToObject(EnumType, ComputeSignedValue());
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Enum.ToObject(EnumType, ComputeUnsignedValue());
                default:
                    throw new NotSupportedException();
            }
        }

        private object ComputeSignedValue()
        {
            long value = 0;
            foreach (var flag in Flags)
            {
                if (flag.IsChecked)
                    value |= Convert.ToInt64(flag.Value);
            }
            return value;
        }

        private ulong ComputeUnsignedValue()
        {
            ulong value = 0;
            foreach (var flag in Flags)
            {
                if (flag.IsChecked)
                    value |= Convert.ToUInt64(flag.Value);
            }
            return value;
        }

        public class Flag : PropertyChangedNotifier
        { 
            public Flag(string name, Enum value, Action checkedCallback)
            {
                Name = name;
                Value = value;
                CheckedCallback = checkedCallback;
            }

            public string Name { get; }
            public Enum Value { get; }
            public bool IsChecked { get; set; }
            public Action CheckedCallback { get; set; }

            public void OnIsCheckedChanged() => CheckedCallback.Invoke();
        }
    }
}
