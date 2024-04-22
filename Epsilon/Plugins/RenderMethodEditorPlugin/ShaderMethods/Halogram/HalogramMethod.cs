using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Halogram;
using System.Collections.Generic;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods.Halogram
{
    class HalogramMethod : MethodParser
    {
        public override string GetMethodName(int methodIndex)
        {
            return ((HalogramMethods)methodIndex).ToString();
        }

        public override string GetOptionName(int methodIndex, int optionIndex)
        {
            switch ((HalogramMethods)methodIndex)
            {
                case HalogramMethods.Albedo:
                    return ((Albedo)optionIndex).ToString();
                case HalogramMethods.Self_Illumination:
                    return ((Self_Illumination)optionIndex).ToString();
                case HalogramMethods.Blend_Mode:
                    return ((Blend_Mode)optionIndex).ToString();
                case HalogramMethods.Misc:
                    return ((Misc)optionIndex).ToString();
                case HalogramMethods.Warp:
                    return ((Warp)optionIndex).ToString();
                case HalogramMethods.Overlay:
                    return ((Overlay)optionIndex).ToString();
                case HalogramMethods.Edge_Fade:
                    return ((Edge_Fade)optionIndex).ToString();
            }
            return "";
        }

        public override string GetOptionDescription(int methodIndex, int optionIndex)
        {
            switch ((HalogramMethods)methodIndex)
            {
                case HalogramMethods.Albedo:
                    var albedo = (Albedo)optionIndex;
                    switch (albedo)
                    {
                        case Albedo.Default:
                            return "Base map combined with a detail map.";
                        case Albedo.Detail_Blend:
                            return "Base map combined with 2 detail maps blended together using the base map alpha channel.";
                        case Albedo.Constant_Color:
                            return "Constant color provided by the albedo color argument.";
                        case Albedo.Two_Change_Color:
                            return "Base map combined with a detail map. Supports two change colors, applied by a color map.";
                        case Albedo.Four_Change_Color:
                            return "Base map combined with a detail map. Supports four change colors, applied by a color map.";
                        case Albedo.Three_Detail_Blend:
                            return "Base map combined with 3 detail maps. Detail maps are blended with the alpha channel of the base map as follows: Values in between 0 and 0.5 blend detail map 1 an 2. Values between 0.5 and 1 blend detail map 2 and 3.";
                        case Albedo.Two_Detail_Overlay:
                            return "Base map combined with 2 detail maps blended together using the base map alpha channel. Overlaid by a an overlay map.";
                        case Albedo.Two_Detail:
                            return "Base map combined with 2 detail maps blended together (alpha blended too).";
                        case Albedo.Color_Mask:
                            return "Base map combined with a detail map. Color is masked by the provided color mask map";
                        case Albedo.Two_Detail_Black_Point:
                            return "Base map combined with 2 detail maps blended together. Alpha modulated by alpha black point calculated via the base and detail maps.";
                    }
                    break;

                case HalogramMethods.Self_Illumination:
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
                        case Self_Illumination.Multilayer_Additive:
                            return "Outputs multiple 4-layers of the self-illumination map to create an illusionary 3D effect.";
                        case Self_Illumination.Ml_Add_Four_Change_Color:
                            return "Outputs multiple 4-layers of the self-illumination map to create an illusionary 3D effect. Supports four change colors.";
                        case Self_Illumination.Ml_Add_Five_Change_Color:
                            return "Outputs multiple 4-layers of the self-illumination map to create an illusionary 3D effect. Supports five change colors.";
                        case Self_Illumination.Scope_Blur:
                            return "Uses multiple samples to blur the self-illumination map, then applies a heat and self-illum color. Typically used with the render method depth camera.";
                        case Self_Illumination.Palettized_Plasma:
                            return "Self-illumination to simulate plasma from palette. U coordinate calculated from 2 noise maps, modulated by depth and alpha. V coordinate specified in the render method tag.";
                    }
                    break;

                case HalogramMethods.Blend_Mode:
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
                    }
                    break;

                case HalogramMethods.Misc:
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

                case HalogramMethods.Warp:
                    var warp = (Warp)optionIndex;
                    switch (warp)
                    {
                        case Warp.None:
                            return "No warping.";
                        case Warp.From_Texture:
                            return "Uses a warp map and amount to offset where each consequent texture is sampled, simulating distortion.";
                        case Warp.Parallax_Simple:
                            return "(UNSUPPORTED) Parallax effect from a single sample of the heightmap";
                    }
                    break;

                case HalogramMethods.Overlay:
                    var overlay = (Overlay)optionIndex;
                    switch (overlay)
                    {
                        case Overlay.None:
                            return "No overlay.";
                        case Overlay.Additive:
                            return "Adds a simple overlay.";
                        case Overlay.Additive_Detail:
                            return "Adds a simple overlay with a detail map.";
                        case Overlay.Multiply:
                            return "Blends an overlay.";
                        case Overlay.Multiply_And_Additive_Detail:
                            return "Blends an overlay, then adds another with a detail map.";
                    }
                    break;

                case HalogramMethods.Edge_Fade:
                    var edge_fade = (Edge_Fade)optionIndex;
                    switch (edge_fade)
                    {
                        case Edge_Fade.None:
                            return "No edge fading.";
                        case Edge_Fade.Simple:
                            return "Simple edge fading with specified tints.";
                    }
                    break;
            }

            return "No description available.";
        }

        public override IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions)
        {
            var albedo = (Albedo)shaderOptions[0].OptionIndex;
            var self_illumination = (Self_Illumination)shaderOptions[1].OptionIndex;
            var blend_mode = (Blend_Mode)shaderOptions[2].OptionIndex;
            var misc = (Misc)shaderOptions[3].OptionIndex;
            var warp = (Warp)shaderOptions[4].OptionIndex;
            var overlay = (Overlay)shaderOptions[5].OptionIndex;
            var edge_fade = (Edge_Fade)shaderOptions[6].OptionIndex;
            var distortion = HaloShaderGenerator.Shared.Distortion.Off;
            var soft_fade = HaloShaderGenerator.Shared.Soft_Fade.Off;

            if (shaderOptions.Count >= 8)
                distortion = (HaloShaderGenerator.Shared.Distortion)shaderOptions[7].OptionIndex;
            if (shaderOptions.Count >= 9)
                soft_fade = (HaloShaderGenerator.Shared.Soft_Fade)shaderOptions[8].OptionIndex;

            return new HalogramGenerator(albedo, self_illumination, blend_mode, misc, warp, overlay, edge_fade, distortion, soft_fade);
        }
    }
}
