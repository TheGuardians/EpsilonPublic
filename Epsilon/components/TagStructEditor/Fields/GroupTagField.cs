using System.Collections.Generic;
using System.Linq;
using TagStructEditor.Common;
using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class GroupTagField : ValueField
    {
        public Tag Value { get; set; }
        public IList<Tag> Tags { get; }

        public GroupTagField(TagList tagList, ValueFieldInfo info) : base(info)
        {
            Tags = tagList.GroupTags;
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            Value = (Tag)value;
        }

        public void OnValueChanged() => SetActualValue(Value);
    }
}
