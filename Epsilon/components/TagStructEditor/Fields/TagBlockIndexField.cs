using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagTool.Tags.Definitions.RenderMethodTemplate;
using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class TagBlockIndexField : ValueField
    {
        public ushort Offset { get; set; }
        public ushort Count { get; set; }

        public TagBlockIndexField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var packedInt = (TagBlockIndex)value;
            Offset = packedInt.Offset;
            Count = packedInt.Count;
        }

        protected void OnOffsetChanged() => UpdateValue();
        protected void OnCountChanged() => UpdateValue();

        void UpdateValue()
        {
            SetActualValue(new TagBlockIndex() { Offset = Offset, Count = Count });
        }
    }
}
