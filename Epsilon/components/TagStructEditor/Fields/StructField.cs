using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace TagStructEditor.Fields
{
    public class StructField : IField
    {
        public ObservableCollection<IField> Fields { get; set; }

        public StructField()
        {
            Fields = new ObservableCollection<IField>();
        }

        public void AddChild(IField child)
        {
            child.Parent = this;
            Fields.Add(child);
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Populate(object owner, object value = null)
        {
            if (value == null)
                value = owner;
                
            foreach (var field in Fields)
                field.Populate(value);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach(var field in Fields)
                field.Dispose();
            Fields.Clear();
        }
    }
}
