using System;

namespace TagStructEditor.Fields
{
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The field the value was changed on
        /// </summary>
        public ValueField Field { get; set; }

        /// <summary>
        /// The old value from before the change
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// The new value after the change
        /// </summary>
        public object NewValue { get; set; }

        public ValueChangedEventArgs(ValueField field, object oldValue, object newValue)
        {
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
