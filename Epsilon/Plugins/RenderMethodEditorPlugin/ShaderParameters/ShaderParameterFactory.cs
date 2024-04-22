using HaloShaderGenerator.Globals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    static class ShaderParameterFactory
    {
        public static ObservableCollection<GenericShaderParameter> BuildShaderParameters(GameCache cache, List<ShaderParameter> parameters, RenderMethod.RenderMethodPostprocessBlock property, RenderMethodTemplate template, bool useRotation=false)
        {
            var result = new ObservableCollection<GenericShaderParameter>();
            foreach(var parameter in parameters)
            {
                if (parameter.RenderMethodExtern != RenderMethodExtern.none)
                    continue;

                var paramName = parameter.ParameterName;
                var desc = FindDescriptionFromName(paramName);

                switch (parameter.RegisterType)
                {
                    case RegisterType.Boolean:
                        int booleanArgumentIndex = FindParameterIndexFromName(cache, template.BooleanParameterNames, paramName);
                        if (booleanArgumentIndex == -1)
                            continue;
                        result.Add(new BooleanShaderParameter(property, paramName, desc, booleanArgumentIndex, cache, template.BooleanParameterNames));
                        break;

                    case RegisterType.Vector:
                        int realArgumentIndex = FindParameterIndexFromName(cache, template.RealParameterNames, paramName);
                        if (realArgumentIndex == -1)
                            continue;

                        bool isCategory = false;
                        if (paramName == "albedo" || paramName == "blend_mode" || paramName == "specialized_rendering" || paramName == "lighting" || paramName == "fog" || paramName == "frame_blend" || paramName == "self_illumination")
                            isCategory = true;

                        switch (parameter.CodeType)
                        {
                            case HLSLType.Float:
                                result.Add(new FloatShaderParameter(property, paramName, desc, realArgumentIndex));
                                break;
                            case HLSLType.Float2:
                                result.Add(new Float2ShaderParameter(property, paramName, desc, realArgumentIndex));
                                break;
                            case HLSLType.Float3:
                                if (parameter.Flags.HasFlag(ShaderParameterFlags.IsColor))
                                    result.Add(new Color3ShaderParameter(property, paramName, desc, realArgumentIndex));
                                else
                                    result.Add(new Float3ShaderParameter(property, paramName, desc, realArgumentIndex));
                                break;

                            case HLSLType.Float4:
                                if (parameter.Flags.HasFlag(ShaderParameterFlags.IsColor))
                                    result.Add(new Color4ShaderParameter(property, paramName, desc, realArgumentIndex));
                                else if (isCategory)
                                    result.Add(new CategoryShaderParameter(property, paramName, desc, realArgumentIndex));
                                else
                                    result.Add(new Float4ShaderParameter(property, paramName, desc, realArgumentIndex));
                                break;
                            case HLSLType.Xform_2d:
                            case HLSLType.Xform_3d:
                                if (useRotation)
                                    result.Add(new TransformRotationShaderParameter(property, paramName, desc, realArgumentIndex));
                                else
                                    result.Add(new TransformShaderParameter(property, paramName, desc, realArgumentIndex));
                                break;
                        }
                        break;

                    case RegisterType.Sampler:
                        int samplerArgumentIndex = FindParameterIndexFromName(cache, template.TextureParameterNames, paramName);
                        if (samplerArgumentIndex == -1)
                            continue;
                        switch (parameter.CodeType)
                        {
                            case HLSLType.Sampler:
                            case HLSLType.sampler2D:
                            case HLSLType.sampler3D:
                                result.Add(new SamplerShaderParameter(property, paramName, desc, samplerArgumentIndex));
                                break;
                        }
                        break;
                }
            }
            return result;
        }

        public static int FindParameterIndexFromName(GameCache cache, List<RenderMethodTemplate.ShaderArgument> arguments, string name)
        {
            int result = -1;

            for(int i = 0; i < arguments.Count; i++)
            {
                var argName = cache.StringTable.GetString(arguments[i].Name);
                if (argName == name)
                {
                    result = i;
                    break;
                }
            }

            if (result == -1)
            {
                Console.WriteLine($"Failed to find argument index for parameter {name}");
            }
                

            return result;
        }

        private static string FindDescriptionFromName(string shaderArgName)
        {
            if (ShaderArgumentsDescription.ArgsDescription.ContainsKey(shaderArgName))
                return ShaderArgumentsDescription.ArgsDescription[shaderArgName];
            else
                return "Missing description";
        }
    }
}
