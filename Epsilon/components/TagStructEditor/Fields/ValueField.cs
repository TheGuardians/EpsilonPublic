using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using TagStructEditor.Common;
using TagStructEditor.Helpers;
using TagTool.Tags;

namespace TagStructEditor.Fields
{
    /// <summary>
    /// Base class for a Field that holds a value
    /// </summary>
    public abstract class ValueField : IField
    {
        public ValueField(ValueFieldInfo info)
        {
            FieldInfo = info;
            Name = Utils.DemangleName(info.Name);
        }

        public ValueFieldInfo FieldInfo { get; set; }

        /// <summary>
        /// Flags for the value field
        /// </summary>
        public ValueFieldFlags FieldFlags => FieldInfo.Flags;

        /// <summary>
        /// The current owner of this field
        /// </summary>
        public object Owner { get; set; }

        /// <summary>
        /// The memory offset of the field relative to the owner
        /// </summary>
        public uint FieldOffset => FieldInfo.Offset;

        /// <summary>
        /// The underlying field's type
        /// </summary>
        public Type FieldType => FieldInfo.FieldType;

        /// <summary>
        /// The formatted name of the field to be displayed
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Flag indicating whether this field should be highlighted
        /// </summary>
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// The type name for display
        /// </summary>
        public string FieldTypeName => FieldType.Name;

        public bool TypeNameIsVisible => FieldFlags.HasFlag(ValueFieldFlags.ShowType);

        protected bool IsPopulating { get; private set; } = false;

        public override void Populate(object owner, object value)
        {
            Owner = owner;

            if (value == null)
                value = FieldInfo.ValueGetter(owner);

            if (value == null)
                value = TryActivate();

            IsPopulating = true;
            OnPopulate(value);
            IsPopulating = false;
        }

        public virtual void SetActualValue(object value)
        {
            // ignore changing the actual value OnPopulate
            if (IsPopulating || FieldInfo.ValueGetter == null || FieldInfo.ValueSetter == null)
                return;

            var oldvalue = FieldInfo.ValueGetter(Owner);

            FieldInfo.ValueSetter(Owner, value);

            FieldInfo.ValueChanged?.Invoke(new ValueChangedEventArgs(this, oldvalue, value));
        }

        private object TryActivate()
        {
            object value = null;

            if (FieldType.IsArray)
            {
                if (FieldInfo.Length > 0)
                    value = Activator.CreateInstance(FieldType, FieldInfo.Length);
                else
                    value = null;
            }
            else
            {
                if (typeof(TagStructure).IsAssignableFrom(FieldType))
                    value = Activator.CreateInstance(FieldType);
            }

            return value;
        }

        /// <summary>
        /// Traverses the field hierarchy, calling the corresponding Visit method on the passed in <see cref="IFieldVisitor"/>
        /// </summary>
        /// <param name="visitor"></param>
        public abstract override void Accept(IFieldVisitor visitor);

        /// <summary>
        /// Called when the field should be populated with the given <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value to populate the field with</param>
        protected abstract void OnPopulate(object value);


        public override string ToString() => $"{Name} : {GetType()}";

        public override void Dispose()
        {
            base.Dispose();
            FieldInfo.Dispose();
        }
    } 
}
