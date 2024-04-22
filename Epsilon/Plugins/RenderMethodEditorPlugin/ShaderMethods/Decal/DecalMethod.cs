using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Decal;
using System.Collections.Generic;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods.Decal
{
    class DecalMethod : MethodParser
    {
        public override string GetMethodName(int methodIndex)
        {
            return ((DecalMethods)methodIndex).ToString();
        }

        public override string GetOptionName(int methodIndex, int optionIndex)
        {
            switch ((DecalMethods)methodIndex)
            {
                case DecalMethods.Albedo:
                    return ((Albedo)optionIndex).ToString();
                case DecalMethods.Blend_Mode:
                    return ((Blend_Mode)optionIndex).ToString();
                case DecalMethods.Render_Pass:
                    return ((Render_Pass)optionIndex).ToString();
                case DecalMethods.Specular:
                    return ((Specular)optionIndex).ToString();
                case DecalMethods.Bump_Mapping:
                    return ((Bump_Mapping)optionIndex).ToString();
                case DecalMethods.Tinting:
                    return ((Tinting)optionIndex).ToString();
            }
            return "";
        }

        public override string GetOptionDescription(int methodIndex, int optionIndex)
        {
            switch ((DecalMethods)methodIndex)
            {
                case DecalMethods.Albedo:
                    var albedo = (Albedo)optionIndex;
                    switch (albedo)
                    {
                        case Albedo.Diffuse_Only:
                            return "Albedo from base map only.";
                        case Albedo.Palettized:
                            return "Albedo from palette, sampled only via base map.";
                        case Albedo.Palettized_Plus_Alpha:
                            return "Albedo from palette, sampled only via base map. Alpha from alpha map.";
                        case Albedo.Diffuse_Plus_Alpha:
                            return "Albedo from base map. Alpha from alpha map.";
                        case Albedo.Emblem_Change_Color:
                            return "Albedo from emblem texture.";
                        case Albedo.Change_Color:
                            return "Albedo from color change map only. Supports three change colors.";
                        case Albedo.Diffuse_Plus_Alpha_Mask:
                            return "Albedo from base map. Alpha from alpha mask map.";
                        case Albedo.Palettized_Plus_Alpha_Mask:
                            return "Albedo from palette, sampled only via base map. Alpha from alpha mask map.";
                        case Albedo.Vector_Alpha:
                            return "Albedo from base map. Alpha from vector map, with applied sharpening + antialias.";
                        case Albedo.Vector_Alpha_Drop_Shadow:
                            return "UNSUPPORTED.";
                    }
                    break;

                case DecalMethods.Blend_Mode:
                    switch ((Blend_Mode)optionIndex)
                    {
                        case Blend_Mode.Opaque:
                            return "No blending";
                        case Blend_Mode.Additive:
                            return "Output of shader added to underlying color";
                        case Blend_Mode.Multiply:
                            return "Output of shader multiplied by underlying color";
                        case Blend_Mode.Double_Multiply:
                            return "Output of shader doubled";
                        case Blend_Mode.Maximum:
                            return "Selects the maximum of the D3D src and dest alpha";
                        case Blend_Mode.Alpha_Blend:
                            return "Alpha output blended with albedo alpha.";
                        case Blend_Mode.Inv_Alpha_Blend:
                            return "Alpha output blended with albedo alpha. D3D src blend factor is inverted.";
                        case Blend_Mode.Pre_Multiplied_Alpha:
                            return "Output of shader is multiplied by alpha component. Alpha output blended with albedo alpha.";
                    }
                    break;

                case DecalMethods.Render_Pass:
                    switch ((Render_Pass)optionIndex)
                    {
                        case Render_Pass.Pre_Lighting:
                            return "Does not expose the decal color.";
                        case Render_Pass.Post_Lighting:
                            return "Exposes the decal color.";
                    }
                    break;

                case DecalMethods.Specular:
                    switch ((Specular)optionIndex)
                    {
                        case Specular.Leave:
                            return "Ignore specular.";
                        case Specular.Modulate:
                            return "Modulates the color by the surface specular (requires decs flag).";
                    }
                    break;

                case DecalMethods.Bump_Mapping:
                    switch ((Bump_Mapping)optionIndex)
                    {
                        case Bump_Mapping.Leave:
                            return "No bump mapping.";
                        case Bump_Mapping.Standard:
                            return "Normal calculated from mesh normal and bump map.";
                        case Bump_Mapping.Standard_Mask:
                            return "UNSUPPORTED.";
                    }
                    break;

                case DecalMethods.Tinting:
                    switch ((Tinting)optionIndex)
                    {
                        case Tinting.None:
                            return "No tinting.";
                        case Tinting.Unmodulated:
                            return "Blends a static tint (after alpha blending).";
                        case Tinting.Partially_Modulated:
                            return "Blends a modulated tint (after alpha blending).";
                        case Tinting.Fully_Modulated:
                            return "Blends a modulated tint (before alpha blending).";
                    }
                    break;
            }

            return "No description available.";
        }

        public override IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions)
        {
            var albedo = (Albedo)shaderOptions[0].OptionIndex;
            var blend_mode = (Blend_Mode)shaderOptions[1].OptionIndex;
            var render_pass = (Render_Pass)shaderOptions[2].OptionIndex;
            var specular = (Specular)shaderOptions[3].OptionIndex;
            var bump_mapping = (Bump_Mapping)shaderOptions[4].OptionIndex;
            var tinting = (Tinting)shaderOptions[5].OptionIndex;

            return new DecalGenerator(albedo, blend_mode, render_pass, specular, bump_mapping, tinting);
        }
    }
}
