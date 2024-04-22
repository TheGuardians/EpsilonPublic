using EpsilonLib.Logging;
using System;
using TagStructEditor.Common;
using TagStructEditor.Helpers;
using TagTool.Cache;
using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class StringIdField : ValueField
    {
        public StringTable _stringTable;

        public string Value { get; set; }
        public string UnicText { get; set; }

        public bool AddButtonEnabled { get; set; }
        public DelegateCommand AddStringIDCommand { get; }

        public StringIdField(StringTable stringTable, ValueFieldInfo info) : base(info)
        {
            _stringTable = stringTable;
            AddStringIDCommand = new DelegateCommand(AddNew, () => AddButtonEnabled);
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var stringId = (StringId)value;
            if (stringId == StringId.Invalid)
                Value = "";
            else
            {
                Value = _stringTable.GetString(stringId);
                UnicText = null;
            }
        }

        public void OnValueChanged()
        {
            if (IsPopulating)
                return;

            StringId stringId = StringId.Invalid;
            if (!string.IsNullOrEmpty(Value))
            {
                stringId = _stringTable.GetStringId(Value);
                if (string.IsNullOrWhiteSpace(Value) || stringId == StringId.Invalid)
                {
                    AddButtonEnabled = true;
                    AddStringIDCommand.RaiseCanExecuteChanged();
                    throw new ArgumentException(nameof(Value));
                }
                else
                {
                    AddButtonEnabled = false;
                    AddStringIDCommand.RaiseCanExecuteChanged();
                }
            }

            SetActualValue(stringId);
        }

        private void AddNew()
        {
            _stringTable.AddString(Value);

            string stringid = Value;
            Value = "default";  // resets failed validation appearance
            Value = stringid;   // set back to added stringid

            Logger.LogCommand(null, null, Logger.CommandEvent.CommandType.none, $"stringid add {Value}");
        }

        public override void Dispose()
        {
            base.Dispose();
            _stringTable = null;
        }
    }
}
