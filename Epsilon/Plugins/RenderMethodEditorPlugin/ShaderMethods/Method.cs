using HaloShaderGenerator.Generator;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods
{
    class Method
    {
        public string MethodName { get; set; }
        public string MethodOption { get; set; }
        public string MethodDescription { get; set; }
        public int MethodIndex { get; set; }
        public int OptionIndex { get; set; }


        public Method(string name, string option, string desc, int methodIndex, int optionIndex)
        {
            MethodName = ShaderStringConverter.ToPrettyFormat(name);
            MethodOption = ShaderStringConverter.ToPrettyFormat(option);
            MethodDescription = desc;
            MethodIndex = methodIndex;
            OptionIndex = optionIndex;
        }
    }

    abstract class MethodParser
    {
        public Method ParseMethod(GameCache cache, RenderMethodDefinition rmdf, int methodIndex, int optionIndex)
        {
            if (methodIndex >= 0 && methodIndex < rmdf.Categories.Count)
            {
                if (optionIndex >= 0 && optionIndex < rmdf.Categories[methodIndex].ShaderOptions.Count)
                {
                    return new Method(cache.StringTable.GetString(rmdf.Categories[methodIndex].Name), cache.StringTable.GetString(rmdf.Categories[methodIndex].ShaderOptions[optionIndex].Name), "", methodIndex, optionIndex);
                }
            }
            return null;
        }

        public abstract string GetMethodName(int methodIndex);
        public abstract string GetOptionName(int methodIndex, int optionIndex);
        public abstract string GetOptionDescription(int methodIndex, int optionIndex);
        public abstract IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions);
    }
}
