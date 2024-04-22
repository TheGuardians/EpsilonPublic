using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData;

namespace TagStructEditor.Fields
{
    public class FunctionTypeRealField : ValueField
    {
        public TagFunction.TagFunctionType FunctionType { get; set; }

        public static readonly Type FunctionTypeEnum = typeof(TagFunction.TagFunctionType);

        public FunctionTypeRealField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var innardsW = (Function.FunctionTypeReal)value;
            FunctionType = (TagFunction.TagFunctionType)innardsW.FunctionType;
        }

        protected void OnFunctionTypeChanged() => UpdateValue();

        void UpdateValue()
        {
            SetActualValue(new Function.FunctionTypeReal() { FunctionType = (float)FunctionType });
        }
    }
}
