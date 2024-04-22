using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;

namespace TagStructEditor.Fields
{
    public class CacheAddressField : ValueField
    {
        public static IReadOnlyCollection<AddressTypeItem> AddressTypes { get; }

        public AddressTypeItem AddressType { get; set; }
        public int AddressOffset { get; set; }

        public CacheAddressField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var address = (CacheAddress)value;
            AddressOffset = address.Offset;
            AddressType = AddressTypes.First(item => item.Type == address.Type);
        }

        private void UpdateValue()
        {
            if(AddressType != null)
                SetActualValue(new CacheAddress(AddressType.Type, AddressOffset));
        }

        public void OnAddressTypeChanged() => UpdateValue();
        public void OnAddressOffsetChanged() => UpdateValue();

        static CacheAddressField()
        {
            var enumType = typeof(CacheAddressType);
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            var addressTypes = new List<AddressTypeItem>();
            for (int i = 0; i < names.Length; i++)
                addressTypes.Add(new AddressTypeItem(names[i], (CacheAddressType)values.GetValue(i)));

            AddressTypes = addressTypes;
        }

        public class AddressTypeItem
        {
            public string Name { get; }
            public CacheAddressType Type { get; }

            public AddressTypeItem(string name, CacheAddressType type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}
