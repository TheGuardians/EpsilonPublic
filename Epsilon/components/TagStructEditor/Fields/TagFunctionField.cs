using TagStructEditor.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFunction;
using System;
using System.Linq;
using TagTool.Common;
using static TagStructEditor.Fields.EnumField;
using System.Collections.ObjectModel;
using TagTool.Cache;
using Stylet;

namespace TagStructEditor.Fields
{
    public class TagFunctionField : DataField, IExpandable
    {
        public bool IsUpdating { get; set; }
        public TagFunction TagFunction { get; set; }

        public EnumMember TypeMember { get; set; }
        public EnumMember OutputTypeMember { get; set; }

        public TagFunctionType Type { get; set; }
        public TagFunctionOutputType OutputType { get; set; }
        public TagFunctionFlags Flags { get; set; }

        public bool IsScalarFunction { get; set; }
        public ScalarFunctionHeader ScalarHeader { get; set; }
        public ColorFunctionHeader ColorHeader { get; set; }

        public float LowerBound { get; set; }
        public float UpperBound { get; set; }

        public ArgbColor Color2 { get; set; }
        public ArgbColor Color3 { get; set; }
        public ArgbColor Color4 { get; set; }
        public ArgbColor Color1 { get; set; }

        public bool Color3Visible { get; set; }
        public bool Color4Visible { get; set; }
        public bool Color1Visible { get; set; }

        public ObservableCollection<EnumMember> FunctionTypeValues { get; }
        public ObservableCollection<EnumMember> FunctionOutputTypeValues { get; }

        public TagFunctionField(ValueFieldInfo info) : base(info)
        {
            var typeinfo = TagEnum.GetInfo(typeof(TagFunctionType), CacheVersion.Unknown, CachePlatform.All);
            FunctionTypeValues = new ObservableCollection<EnumMember>(GenerateMemberList(typeinfo));

            var outputtypeinfo = TagEnum.GetInfo(typeof(TagFunctionOutputType), CacheVersion.Unknown, CachePlatform.All);
            FunctionOutputTypeValues = new ObservableCollection<EnumMember>(GenerateMemberList(outputtypeinfo));
        }

        protected override void OnPopulate(object value)
        {
            TagFunction = (TagFunction)value;
            base.OnPopulate(TagFunction.Data);

            if (Data != null && Data.Length > 0)
                UpdateHeaderFromData();
        }

        public override void SetActualValue(object value)
        {
            base.SetActualValue(new TagFunction() { Data = (byte[])value });
        }

        public void UpdateHeaderFromData()
        {
            Type = (TagFunctionType)Data[0];
            TypeMember = FunctionTypeValues.FirstOrDefault(m => m.Value.Equals(Type));

            Flags = (TagFunctionFlags)Data[1];

            OutputType = (TagFunctionOutputType)Data[2];
            OutputTypeMember = FunctionOutputTypeValues.FirstOrDefault(m => m.Value.Equals(OutputType));

            IsScalarFunction = OutputType == TagFunctionOutputType.Scalar;

            if (IsScalarFunction)
            {
                LowerBound = BitConverter.ToSingle(Data, 4);
                UpperBound = BitConverter.ToSingle(Data, 8);

                //ScalarHeader = new ScalarFunctionHeader()
                //{
                //    Type = Type,
                //    Flags = Flags,
                //    OutputType = TagFunctionOutputType.Scalar,
                //    ScaleBounds = new Bounds<float>()
                //    {
                //        Lower = BitConverter.ToSingle(Data, 4),
                //        Upper = BitConverter.ToSingle(Data, 8)
                //    },
                //    Point = new RealPoint2d()
                //    {
                //        X = BitConverter.ToSingle(Data, 20),
                //        Y = BitConverter.ToSingle(Data, 24)
                //    },
                //    Capacity = BitConverter.ToUInt32(Data, 28)
                //};
            }
            else
            {
                Color2 = ToArgbColor(Data, 4);
                Color3 = ToArgbColor(Data, 8);
                Color4 = ToArgbColor(Data, 12);
                Color1 = ToArgbColor(Data, 16);

                //ColorHeader = new ColorFunctionHeader()
                //{
                //    Type = Type,
                //    Flags = Flags,
                //    OutputType = OutputType,
                //    Color2 = new ArgbColor(BitConverter.ToUInt32(Data, 4)),
                //    Color3 = new ArgbColor(BitConverter.ToUInt32(Data, 8)),
                //    Color4 = new ArgbColor(BitConverter.ToUInt32(Data, 12)),
                //    Color1 = new ArgbColor(BitConverter.ToUInt32(Data, 16)),
                //    Point = new RealPoint2d()
                //    {
                //        X = BitConverter.ToSingle(Data, 20),
                //        Y = BitConverter.ToSingle(Data, 24)
                //    },
                //    Capacity = BitConverter.ToUInt32(Data, 28)
                //};
            }
        }

        private ArgbColor ToArgbColor(byte[] data, int i)
        {
            return new ArgbColor() { Alpha = 255, Blue = data[i], Green = data[i + 1], Red = data[i + 2] };
        }

        public void ReplaceDataFloat(float value, int index)
        {
            if ( IsPopulating || IsUpdating || Data == null || Data.Length < index + 4)
                return;

            byte[] bytes = BitConverter.GetBytes(value);
            byte[] temp = Data.ToArray();

            Array.Copy(bytes, 0, temp, index, 4);
            Data = temp;
        }

        public void ReplaceData(byte[] valueBytes, int index)
        {
            if (IsPopulating || IsUpdating || Data == null || Data.Length < index + valueBytes.Length)
                return;

            byte[] temp = Data.ToArray();

            Array.Copy(valueBytes, 0, temp, index, valueBytes.Length);
            Data = temp;
        }

        public void ReplaceDataEnumMember(EnumMember member, int index)
        {
            if (IsPopulating || IsUpdating || Data == null || Data.Length < index + 1)
                return;

            byte value = Convert.ToByte(member.Value);

            byte[] temp = Data.ToArray();

            temp[index] = value;
            Data = temp;
        }

        public void ReplaceDataARGB(ArgbColor color, int index)
        {
            if (IsPopulating || IsUpdating || Data == null || Data.Length < index + 4)
                return;

            byte[] valueBytes = new byte[] { color.Blue, color.Green, color.Red, 0 };
            byte[] temp = Data.ToArray();

            Array.Copy(valueBytes, 0, temp, index, 4);
            Data = temp;
        }

        public void OnTypeMemberChanged() => ReplaceDataEnumMember(TypeMember, 0);
        //public void OnFlagsChanged() => ReplaceData(BitConverter.GetBytes((byte)Flags), 1);
        public void OnOutputTypeMemberChanged()
        {
            ReplaceDataEnumMember(OutputTypeMember, 2);

            if ((TagFunctionOutputType)OutputTypeMember.Value == TagFunctionOutputType.Scalar)
                IsScalarFunction = true;
            else
            {
                IsScalarFunction = false;

                var temp = (TagFunctionOutputType)OutputTypeMember.Value;
                Color1Visible = temp >= TagFunctionOutputType.TwoColor;
                Color3Visible = temp >= TagFunctionOutputType.ThreeColor;
                Color4Visible = temp >= TagFunctionOutputType.FourColor;
            }
        }

        public void OnLowerBoundChanged() => ReplaceDataFloat(LowerBound, 4);
        public void OnUpperBoundChanged() => ReplaceDataFloat(UpperBound, 8);

        public void OnColor2Changed() => ReplaceDataARGB(Color2, 4);
        public void OnColor3Changed() => ReplaceDataARGB(Color3, 8);
        public void OnColor4Changed() => ReplaceDataARGB(Color4, 12);
        public void OnColor1Changed() => ReplaceDataARGB(Color1, 16);

        public override void OnDataChanged()
        {
            SetActualValue(Data);
        
            RaisePropertyChanged(nameof(Length));
            ImportCommand.RaiseCanExecuteChanged();
            ExportCommand.RaiseCanExecuteChanged();
        
            if(!IsPopulating)
            {
                IsUpdating = true;
                UpdateHeaderFromData();
                IsUpdating = false;
            }
        }
    }
}
