using EpsilonLib.Logging;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    class BooleanShaderParameter : GenericShaderParameter
    {
        private List<string> booleanArgStrings = new List<string>();
        public bool Value
        {
            get => (((int)(Property.BooleanConstants) >> TemplateIndex) & 1) == 1;
            set
            {
                if (value == true)
                    Property.BooleanConstants = (uint)((int)Property.BooleanConstants | (1 << TemplateIndex));
                else
                    Property.BooleanConstants = (uint)((int)Property.BooleanConstants & ~(1 << TemplateIndex));
                List<string> setStrings = new List<string>();
                for(var i = 0; i < booleanArgStrings.Count; i++)
                {
                    if ((Property.BooleanConstants & 1 << i) != 0)
                    {
                        setStrings.Add(booleanArgStrings[i]);
                    }
                }

                string argstring = setStrings.Count == 0 ? "none" : string.Join(",", setStrings);

                Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", "booleans",
                    Logger.CommandEvent.CommandType.setfield, $"setargument booleans {argstring}");
            }
        }

        public BooleanShaderParameter(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex, GameCache cache, List<RenderMethodTemplate.ShaderArgument> booleanArgs) : base(property, name, desc, templateIndex)
        {
            foreach(var arg in booleanArgs)
            {
                booleanArgStrings.Add(cache.StringTable.GetString(arg.Name));
            }
        }
    }
}
