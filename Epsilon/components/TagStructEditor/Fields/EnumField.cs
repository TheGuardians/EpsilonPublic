using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagStructEditor.Helpers;
using TagTool.Cache;
using TagTool.Tags;

namespace TagStructEditor.Fields
{
    public class EnumField : ValueField
    {
        public EnumMember Value { get; set; }
        public ObservableCollection<EnumMember> Values { get; }

        public EnumField(ValueFieldInfo info, TagEnumInfo enumInfo) : base(info)
        {
            Values = new ObservableCollection<EnumMember>(GenerateMemberList(enumInfo));
        }

        public static IEnumerable<EnumMember> GenerateMemberList(TagEnumInfo info)
        {
            var members = TagEnum.GetMemberEnumerable(info).Members;
            for (int i = 0; i < members.Count; i++)
                yield return new EnumMember(Utils.DemangleName(members[i].Name), (Enum)members[i].Value);
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            Value = Values.FirstOrDefault(member => member.Value.Equals((Enum)value));
        }

        public void OnValueChanged() => SetActualValue(Value?.Value);

        public class EnumMember
        {
            public string Name { get; }
            public Enum Value { get; }

            public EnumMember(string name, Enum value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
