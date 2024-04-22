using EpsilonLib.Logging;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    class FloatShaderParameter : GenericShaderParameter
    {
        public float Value
        {
            get => Property.RealConstants[TemplateIndex].Arg0;
            set
            {
                Property.RealConstants[TemplateIndex].Arg0 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield, $"setargument {Name} {value}");
            }               
        }

        public FloatShaderParameter(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex) : base(property, name, desc, templateIndex)
        {
        }
    }
}
