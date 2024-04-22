using TagStructEditor.Common;
using TagTool.Tags;

namespace TagStructEditor.Fields
{
    public class TagDataField : DataField, IExpandable
    {
        public TagDataField(ValueFieldInfo info) : base(info)
        {
        }

        protected override void OnPopulate(object value)
        {
            var data = (TagData)value;
            base.OnPopulate(data.Data);
        }

        public override void SetActualValue(object value)
        {
            base.SetActualValue(new TagData() { Data = (byte[])value });
        }

        public override void Dispose()
        {
            base.Dispose();
            Data = null;
        }
    }
}
