using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagStructEditor.Common;
using TagStructEditor.Helpers;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagStructEditor.Fields
{
    public class FlagBitsField : ValueField
    {
        public FlagBitsField(ValueFieldInfo info, CacheVersion version, CachePlatform platform) : base(info)
        {
            var enumType = info.FieldType.GenericTypeArguments[0];
            var enumInfo = TagEnum.GetInfo(enumType, version, platform);
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
            //visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var valueEnum = (IFlagBits)value;

            foreach (var member in Flags)
                member.IsChecked = valueEnum.TestBit((Enum)member.Value);
        }

        private void OnFlagToggled()
        {
            if (!IsPopulating)
                SetActualValue(ComputeValue());
        }

        private object ComputeValue()
        {
            var flagBits = (IFlagBits)Activator.CreateInstance(FieldInfo.FieldType.GetGenericTypeDefinition().MakeGenericType(EnumType));
            foreach (var flag in Flags)
            {
                if (flag.IsChecked)
                    flagBits.SetBit(flag.Value, true);
            }
            return flagBits;
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
