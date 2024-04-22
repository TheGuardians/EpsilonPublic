using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData;
using static TagTool.Tags.ParticlePropertyScalar;

namespace TagStructEditor.Fields
{
    public class EmitterGpuInnardsYField : ValueField
    {
        public byte FunctionIndexRed { get; set; }
        public ParticleStates InputIndexRed { get; set; }
        public byte IsConstant { get; set; }

        public static readonly Type ParticleStatesEnum = typeof(ParticleStates);

        public EmitterGpuInnardsYField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var innardsY = (Property.InnardsY)value;
            FunctionIndexRed = innardsY.FunctionIndexRed;
            InputIndexRed = innardsY.InputIndexRed;
            IsConstant = innardsY.IsConstant;
        }

        protected void OnFunctionIndexRedChanged() => UpdateValue();
        protected void OnInputIndexRedChanged() => UpdateValue();
        protected void OnIsConstantChanged() => UpdateValue();

        void UpdateValue()
        {
            SetActualValue(new Property.InnardsY() { FunctionIndexRed = FunctionIndexRed, InputIndexRed = InputIndexRed, IsConstant = IsConstant });
        }
    }
}
