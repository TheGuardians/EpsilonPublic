using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Screen;
using System.Collections.Generic;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods.Screen
{
    class ScreenMethod : MethodParser
    {
        public override string GetMethodName(int methodIndex)
        {
            return ((ScreenMethods)methodIndex).ToString();
        }

        public override string GetOptionName(int methodIndex, int optionIndex)
        {
            switch ((ScreenMethods)methodIndex)
            {
                case ScreenMethods.Warp:
                    return ((Warp)optionIndex).ToString();
                case ScreenMethods.Base:
                    return ((Base)optionIndex).ToString();
                case ScreenMethods.Overlay_A:
                    return ((Overlay_A)optionIndex).ToString();
                case ScreenMethods.Overlay_B:
                    return ((Overlay_B)optionIndex).ToString();
                case ScreenMethods.Blend_Mode:
                    return ((Blend_Mode)optionIndex).ToString();
            }
            return "";
        }

        public override string GetOptionDescription(int methodIndex, int optionIndex)
        {
            switch ((ScreenMethods)methodIndex)
            {
                case ScreenMethods.Warp:
                    switch ((Warp)optionIndex)
                    {
                        case Warp.None:
                            return "No warping.";
                        case Warp.Pixel_Space:
                            return "Warp using pixel space.";
                        case Warp.Screen_Space:
                            return "Warp using screen space.";
                    }
                    break;
                case ScreenMethods.Base:
                    switch ((Base)optionIndex)
                    {
                        case Base.Single_Pixel_Space:
                            return "Sample basemap within pixel space.";
                        case Base.Single_Screen_Space:
                            return "Sample basemap within screen space.";
                    }
                    break;
                case ScreenMethods.Overlay_A:
                    switch ((Overlay_A)optionIndex)
                    {
                        case Overlay_A.None:
                            return "No overlay.";
                        case Overlay_A.Tint_Add_Color:
                            return "Tinted overlay with an added color.";
                        case Overlay_A.Detail_Screen_Space:
                            return "Overlays a simple detail map within screen space.";
                        case Overlay_A.Detail_Pixel_Space:
                            return "Overlays a simple detail map within pixel space.";
                        case Overlay_A.Detail_Masked_Screen_Space:
                            return "Overlays an alpha masked detail map within screen space.";
                    }
                    break;
                case ScreenMethods.Overlay_B:
                    switch ((Overlay_B)optionIndex)
                    {
                        case Overlay_B.None:
                            return "No overlay.";
                        case Overlay_B.Tint_Add_Color:
                            return "Tinted overlay with an added color.";
                    }
                    break;
                case ScreenMethods.Blend_Mode:
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
                        case Blend_Mode.Alpha_Blend:
                            return "Alpha output blended with albedo alpha.";
                        case Blend_Mode.Pre_Multiplied_Alpha:
                            return "Output of shader is multiplied by alpha component. Alpha output blended with albedo alpha.";
                    }
                    break;
            }

            return "No description available.";
        }

        public override IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions)
        {
            var warp = (Warp)shaderOptions[0].OptionIndex;
            var _base = (Base)shaderOptions[1].OptionIndex;
            var overlay_a = (Overlay_A)shaderOptions[2].OptionIndex;
            var overlay_b = (Overlay_B)shaderOptions[3].OptionIndex;
            var blend_mode = (Blend_Mode)shaderOptions[4].OptionIndex;

            return new ScreenGenerator(warp, _base, overlay_a, overlay_b, blend_mode);
        }
    }
}
