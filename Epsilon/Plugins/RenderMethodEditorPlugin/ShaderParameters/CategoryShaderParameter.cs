using EpsilonLib.Logging;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    class CategoryShaderParameter : GenericShaderParameter
    {
        public int Value
        {
            get => (int)Property.RealConstants[TemplateIndex].Arg0;
            set 
            {
                Property.RealConstants[TemplateIndex].Arg0 = value;
                Property.RealConstants[TemplateIndex].Arg1 = value;
                Property.RealConstants[TemplateIndex].Arg2 = value;
                Property.RealConstants[TemplateIndex].Arg3 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield, $"setargument {Name} {value} {value} {value} {value}");
            } 
        }

        public CategoryShaderParameter(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex) : base(property, name, desc, templateIndex)
        {
        }
    }
}
