using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData;
using static TagTool.Tags.ParticlePropertyScalar;

namespace TagStructEditor.Fields
{
    public class EmitterGpuInnardsWField : ValueField
    {
        public byte ColorIndexLo { get; set; }
        public byte ColorIndexHi { get; set; }
        public ParticleStates InputIndexGreen { get; set; }

        public static readonly Type ParticleStatesEnum = typeof(ParticleStates);

        public EmitterGpuInnardsWField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var innardsW = (Property.InnardsW)value;
            ColorIndexLo = innardsW.ColorIndexLo;
            ColorIndexHi = innardsW.ColorIndexHi;
            InputIndexGreen = innardsW.InputIndexGreen;
        }

        protected void OnColorIndexLoChanged() => UpdateValue();
        protected void OnColorIndexHiChanged() => UpdateValue();
        protected void OnInputIndexGreenChanged() => UpdateValue();

        void UpdateValue()
        {
            SetActualValue(new Property.InnardsW() { ColorIndexLo = ColorIndexLo, ColorIndexHi = ColorIndexHi, InputIndexGreen = InputIndexGreen });
        }
    }
}
