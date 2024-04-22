using EpsilonLib.Logging;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    class TransformShaderParameter : GenericShaderParameter
    {
        public float Value1
        {
            get => Property.RealConstants[TemplateIndex].Arg0;
            set
            {
                Property.RealConstants[TemplateIndex].Arg0 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield,
                    $"setargument {Name} {value} {Property.RealConstants[TemplateIndex].Arg1} " +
                    $"{Property.RealConstants[TemplateIndex].Arg2} {Property.RealConstants[TemplateIndex].Arg3}");
            }
        }

        public float Value2
        {
            get => Property.RealConstants[TemplateIndex].Arg1;
            set
            {
                Property.RealConstants[TemplateIndex].Arg1 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield,
                    $"setargument {Name} {Property.RealConstants[TemplateIndex].Arg0} {value} " +
                    $"{Property.RealConstants[TemplateIndex].Arg2} {Property.RealConstants[TemplateIndex].Arg3}");
            }
        }

        public float Value3
        {
            get => Property.RealConstants[TemplateIndex].Arg2;
            set
            {
                Property.RealConstants[TemplateIndex].Arg2 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield,
                    $"setargument {Name} {Property.RealConstants[TemplateIndex].Arg0} {Property.RealConstants[TemplateIndex].Arg1} " +
                    $"{value} {Property.RealConstants[TemplateIndex].Arg3}");
            }
        }

        public float Value4
        {
            get => Property.RealConstants[TemplateIndex].Arg3;
            set
            {
                Property.RealConstants[TemplateIndex].Arg3 = value;
                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", Name,
                    Logger.CommandEvent.CommandType.setfield,
                    $"setargument {Name} {Property.RealConstants[TemplateIndex].Arg0} {Property.RealConstants[TemplateIndex].Arg1} " +
                    $"{Property.RealConstants[TemplateIndex].Arg2} {value}");
            }
        }

        public TransformShaderParameter(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex) : base(property, name + " transform", desc, templateIndex)
        {
        }
    }
}
