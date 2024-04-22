using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Shader;
using System.Collections.Generic;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods.Shader
{
    class ShaderMethod : MethodParser
    {
        public override string GetMethodName(int methodIndex)
        {
            return ((HaloShaderGenerator.Shader.ShaderMethods)methodIndex).ToString();
        }

        public override string GetOptionName(int methodIndex, int optionIndex)
        {
            switch ((HaloShaderGenerator.Shader.ShaderMethods)methodIndex)
            {
                case HaloShaderGenerator.Shader.ShaderMethods.Albedo:
                    return ((Albedo)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Bump_Mapping:
                    return ((Bump_Mapping)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Alpha_Test:
                    return ((Alpha_Test)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Specular_Mask:
                    return ((Specular_Mask)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Material_Model:
                    return ((Material_Model)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Environment_Mapping:
                    return ((Environment_Mapping)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Self_Illumination:
                    return ((Self_Illumination)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Blend_Mode:
                    return ((Blend_Mode)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Parallax:
                    return ((Parallax)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Misc:
                    return ((Misc)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Distortion:
                    return ((HaloShaderGenerator.Shared.Distortion)optionIndex).ToString();
                case HaloShaderGenerator.Shader.ShaderMethods.Soft_Fade:
                    return ((HaloShaderGenerator.Shared.Soft_Fade)optionIndex).ToString();
            }
            return "";
        }

        public override string GetOptionDescription(int methodIndex, int optionIndex)
        {
            switch ((HaloShaderGenerator.Shader.ShaderMethods)methodIndex)
            {
                case HaloShaderGenerator.Shader.ShaderMethods.Albedo:
                    var albedo = (Albedo)optionIndex;
                    switch (albedo)
                    {
                        case Albedo.Default:
                            return "Base map combined with a detail map.";
                        case Albedo.Detail_Blend:
                            return "Base map combined with 2 detail maps blended together using the base map alpha channel.";
                        case Albedo.Constant_Color:
                            return "Constant color provided by the albedo color argument.";

                        case Albedo.Three_Detail_Blend:
                            return "Base map combined with 3 detail maps. Detail maps are blended with the alpha channel of the base map as follows: Values in between 0 and 0.5 blend detail map 1 an 2. Values between 0.5 and 1 blend detail map 2 and 3.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Bump_Mapping:
                    var bump_mapping = (Bump_Mapping)optionIndex;
                    switch (bump_mapping)
                    {
                        case Bump_Mapping.Off:
                            return "Normal comes from mesh without any modification.";
                        case Bump_Mapping.Standard:
                            return "Normal calculated from mesh normal and bump map.";
                        case Bump_Mapping.Detail:
                            return "Normal calculated from mesh normal and bump map. Bump detail added over resulting normal.";
                        case Bump_Mapping.Detail_Masked:
                            return "Normal calculated from mesh normal and bump map. Masked bump detail added over resulting normal.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Alpha_Test:
                    var alpha_test = (Alpha_Test)optionIndex;
                    switch (alpha_test)
                    {
                        case Alpha_Test.Simple:
                            return "Transparency testing from alpha map.";
                        case Alpha_Test.None:
                            return "No transparency";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Specular_Mask:
                    var specular_mask = (Specular_Mask)optionIndex;
                    switch (specular_mask)
                    {
                        case Specular_Mask.No_Specular_Mask:
                            return "No specular mask";
                        case Specular_Mask.Specular_Mask_From_Diffuse:
                            return "Specular mask provided by alpha channel from base map.";
                        case Specular_Mask.Specular_Mask_From_Texture:
                            return "Specular mask provided by specular map.";
                        case Specular_Mask.Specular_Mask_From_Color_Texture:
                            return "Specular mask provided by color specular map.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Material_Model:
                    var material_model = (Material_Model)optionIndex;
                    switch (material_model)
                    {
                        case Material_Model.Diffuse_Only:
                            return "Material uses only Lambertian diffuse.";
                        case Material_Model.Cook_Torrance:
                            return "Cook-Torrance BRDF for area and analytical specular. Lambertian for diffuse component.";
                        case Material_Model.Two_Lobe_Phong:
                            return "Phong BRDF with glancing and specular reflections.";
                        case Material_Model.None:
                            return "No material model, albedo only.";
                        case Material_Model.Glass:
                            return "Phong optimized for transparent materials";
                        case Material_Model.Single_Lobe_Phong:
                            return "Phong BRDF";
                        case Material_Model.Organism:
                            return "Material model combining subsurface scattering, occlusion, transparence and rim light for organic surfaces like skin.";
                        case Material_Model.Foliage:
                            return "Variant of diffuse only material.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Environment_Mapping:
                    var env_map = (Environment_Mapping)optionIndex;
                    switch (env_map)
                    {
                        case Environment_Mapping.None:
                            return "No environmental map contribution";
                        case Environment_Mapping.Per_Pixel:
                            return "Single cubemap environmental contribution.";
                        case Environment_Mapping.Dynamic:
                            return "Environmental map contribution from 2 cubemaps dynamically changed.";
                        case Environment_Mapping.Custom_Map:
                            return "Environmental contribution from custom texture";
                        case Environment_Mapping.From_Flat_Texture:
                            return "Envrionmental contribution from flat texture. Used for simulating real time reflections.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Self_Illumination:
                    var self_illum = (Self_Illumination)optionIndex;
                    switch (self_illum)
                    {
                        case Self_Illumination.Off:
                            return "No self-illumination";
                        case Self_Illumination.Simple:
                            return "Self-illumination from texture.";
                        case Self_Illumination._3_Channel_Self_Illum:
                            return "Self-illumination created from 3 colors and a mask map.";
                        case Self_Illumination.Plasma:
                            return "Self-illumination to simulate plasma. Combination of 2 noise maps, thinness parameters and alpha map.";
                        case Self_Illumination.From_Diffuse:
                            return "Self-illumination applied to diffuse color directly.";
                        case Self_Illumination.Illum_Detail:
                            return "Self-illumination from texture and detail map.";
                        case Self_Illumination.Meter:
                            return "Self-illumination from meter texture. Controlled by meter_value argument and offset by alpha channel of meter map";
                        case Self_Illumination.Self_Illum_Times_Diffuse:
                            return "Self-illumination blended with diffuse color";
                        case Self_Illumination.Simple_With_Alpha_Mask:
                            return "Self-illumination from texture with mask in alpha channel";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Blend_Mode:
                    var blend_mode = (Blend_Mode)optionIndex;
                    switch (blend_mode)
                    {
                        case Blend_Mode.Opaque:
                            return "No blending";
                        case Blend_Mode.Additive:
                            return "Output of shader added to underlying color";
                        case Blend_Mode.Multiply:
                            return "Output of shader multiplied by underlying color";
                        case Blend_Mode.Double_Multiply:
                            return "Output of shader doubled";
                        case Blend_Mode.Alpha_Blend:
                            return "Alpha output blended with albedo alpha.";
                        case Blend_Mode.Pre_Multiplied_Alpha:
                            return "Output of shader is multiplied by alpha component. Alpha output blended with albedo alpha.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Parallax:
                    var parallax = (Parallax)optionIndex;
                    switch (parallax)
                    {
                        case Parallax.Off:
                            return "No parallax";
                        case Parallax.Simple:
                            return "Parallax effect from a single sample of the heightmap";
                        case Parallax.Interpolated:
                            return "Parallax effect interpolated from 2 samples of the heightmap";
                        case Parallax.Simple_Detail:
                            return "Parallax effect from a single sample of the heightmap, scaled by scale map.";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Misc:
                    var misc = (Misc)optionIndex;
                    switch (misc)
                    {
                        case Misc.First_Person_Always:
                        case Misc.First_Person_Sometimes:
                            return "Two pass shader, use albedo pass to obtain albedo values.";
                        case Misc.First_Person_Never:
                            return "Single pass shader, compute albedo pass inside lighting pass.";
                        case Misc.First_Person_Never_With_rotating_Bitmaps:
                            return "Single pass shader, compute albedo pass inside lighting pass. Texture map transforms are (angle, scale, offset x, offset y)";
                    }
                    break;

                case HaloShaderGenerator.Shader.ShaderMethods.Distortion:
                    var dist = (HaloShaderGenerator.Shared.Distortion)optionIndex;
                    switch (dist)
                    {
                        case HaloShaderGenerator.Shared.Distortion.Off:
                            return "No distortion effect";
                    }
                    break;

            }
            return "No description available.";
        }

        public override IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions)
        {
            var albedo = (Albedo)shaderOptions[0].OptionIndex;
            var bump_mapping = (Bump_Mapping)shaderOptions[1].OptionIndex;
            var alpha_test = (Alpha_Test)shaderOptions[2].OptionIndex;
            var specular_mask = (Specular_Mask)shaderOptions[3].OptionIndex;
            var material_model = (Material_Model)shaderOptions[4].OptionIndex;
            var environment_mapping = (Environment_Mapping)shaderOptions[5].OptionIndex;
            var self_illumination = (Self_Illumination)shaderOptions[6].OptionIndex;
            var blend_mode = (Blend_Mode)shaderOptions[7].OptionIndex;
            var parallax = (Parallax)shaderOptions[8].OptionIndex;
            var misc = (Misc)shaderOptions[9].OptionIndex;
            var distortion = HaloShaderGenerator.Shared.Distortion.Off;
            var soft_fade = HaloShaderGenerator.Shared.Soft_Fade.Off;

            if (shaderOptions.Count >= 11)
                distortion = (HaloShaderGenerator.Shared.Distortion)shaderOptions[10].OptionIndex;
            if (shaderOptions.Count >= 12)
                soft_fade = (HaloShaderGenerator.Shared.Soft_Fade)shaderOptions[11].OptionIndex;

            return new ShaderGenerator(albedo, bump_mapping, alpha_test, specular_mask, material_model, environment_mapping, self_illumination, blend_mode, parallax, misc, distortion, soft_fade);
        }
    }
}
