using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagStructEditor.Common;

namespace TagStructEditor.Fields
{
    public class InlineStructField : ValueField, IExpandable
    {
        public ObservableCollection<IField> Fields { get; set; }
        public bool IsExpanded { get; set; }

        public InlineStructField(ValueFieldInfo info) : base(info)
        {
            Fields = new ObservableCollection<IField>();
        }

        public void AddChild(IField child)
        {
            child.Parent = this;
            Fields.Add(child);
        }

        protected override void OnPopulate(object value)
        {
            foreach (var field in Fields)
                field.Populate(value);
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var field in Fields)
                field.Dispose();
            Fields.Clear();
        }
    }
}
