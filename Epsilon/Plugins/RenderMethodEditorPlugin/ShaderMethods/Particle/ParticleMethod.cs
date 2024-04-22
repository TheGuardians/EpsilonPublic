using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Particle;
using System.Collections.Generic;
using static TagTool.Tags.Definitions.RenderMethod;

namespace RenderMethodEditorPlugin.ShaderMethods.Particle
{
    class ParticleMethod : MethodParser
    {
        public override string GetMethodName(int methodIndex)
        {
            return ((ParticleMethods)methodIndex).ToString();
        }

        public override string GetOptionName(int methodIndex, int optionIndex)
        {
            switch ((ParticleMethods)methodIndex)
            {
                case ParticleMethods.Albedo:
                    return ((Albedo)optionIndex).ToString();
                case ParticleMethods.Blend_Mode:
                    return ((Blend_Mode)optionIndex).ToString();
                case ParticleMethods.Specialized_Rendering:
                    return ((Specialized_Rendering)optionIndex).ToString();
                case ParticleMethods.Lighting:
                    return ((Lighting)optionIndex).ToString();
                case ParticleMethods.Render_Targets:
                    return ((Render_Targets)optionIndex).ToString();
                case ParticleMethods.Depth_Fade:
                    return ((Depth_Fade)optionIndex).ToString();
                case ParticleMethods.Black_Point:
                    return ((Black_Point)optionIndex).ToString();
                case ParticleMethods.Fog:
                    return ((Fog)optionIndex).ToString();
                case ParticleMethods.Frame_Blend:
                    return ((Frame_Blend)optionIndex).ToString();
                case ParticleMethods.Self_Illumination:
                    return ((Self_Illumination)optionIndex).ToString();
            }
            return "";
        }

        public override string GetOptionDescription(int methodIndex, int optionIndex)
        {
            switch ((ParticleMethods)methodIndex)
            {
                case ParticleMethods.Albedo:
                    switch ((Albedo)optionIndex)
                    {
                        case Albedo.Diffuse_Only:
                            return "Albedo from base map only.";
                        case Albedo.Diffuse_Plus_Billboard_Alpha:
                            return "Albedo RGB from base map and alpha from billboard alpha map.";
                        case Albedo.Palettized:
                            return "Albedo from palette - the base map's red channel is used as the U coordinate (V predetermined).";
                        case Albedo.Palettized_Plus_Billboard_Alpha:
                            return "Albedo RGB from palette - the base map's red channel is used as the U coordinate (V predetermined). Alpha is sampled from billboard alpha map.";
                        case Albedo.Diffuse_Plus_Sprite_Alpha:
                            return "Albedo RGB from base map, alpha from alpha spritesheet.";
                        case Albedo.Palettized_Plus_Sprite_Alpha:
                            return "Albedo RGB from palette - the base map's red channel is used as the U coordinate (V predetermined). Alpha from alpha spritesheet.";
                        case Albedo.Diffuse_Modulated:
                            return "Albedo from base map only, with RGB modulated by tint color.";
                        case Albedo.Palettized_Glow:
                            return "UNSUPPORTED.";
                        case Albedo.Palettized_Plasma:
                            return "Albedo from palette - U coordinate determined by basemap1 - basemap2, modulated by a calculated alpha modulation factor (V predetermined).";
                    }
                    break;

                case ParticleMethods.Blend_Mode:
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

                case ParticleMethods.Specialized_Rendering:
                    switch ((Specialized_Rendering)optionIndex)
                    {
                        case Specialized_Rendering.None:
                            return "No specialized rendering.";
                        case Specialized_Rendering.Distortion:
                            return "Output is distortion.";
                        case Specialized_Rendering.Distortion_Expensive:
                            return "Output is distortion. Expensive depth testing is used.";
                        case Specialized_Rendering.Distortion_Diffuse:
                            return "Output is distortion, explicitly determined by the albedo output.";
                        case Specialized_Rendering.Distortion_Expensive_Diffuse:
                            return "Output is distortion, explicitly determined by the albedo output. Expensive depth testing is used.";
                    }
                    break;

                case ParticleMethods.Lighting:
                    switch ((Lighting)optionIndex)
                    {
                        case Lighting.None:
                            return "No lighting applied.";
                        case Lighting.Per_Pixel_Ravi_Order_3:
                            return "Use Order3 per pixel lighting.";
                        case Lighting.Per_Vertex_Ravi_Order_0:
                            return "Use Order0 per vertex lighting.";
                    }
                    break;

                case ParticleMethods.Render_Targets:
                    switch ((Render_Targets)optionIndex)
                    {
                        case Render_Targets.Ldr_And_Hdr:
                            return "Output is rendered to LDR and HDR.";
                        case Render_Targets.Ldr_Only:
                            return "Output is rendered to LDR only.";
                    }
                    break;

                case ParticleMethods.Depth_Fade:
                    switch ((Depth_Fade)optionIndex)
                    {
                        case Depth_Fade.Off:
                            return "No depth fadeout.";
                        case Depth_Fade.On:
                            return "Output fades by the specified depth fade range.";
                    }
                    break;

                case ParticleMethods.Black_Point:
                    switch ((Black_Point)optionIndex)
                    {
                        case Black_Point.Off:
                            return "No alpha black point modulation.";
                        case Black_Point.On:
                            return "Output alpha is modulated by the alpha black point.";
                    }
                    break;

                case ParticleMethods.Fog:
                    switch ((Fog)optionIndex)
                    {
                        case Fog.Off:
                            return "No fog.";
                        case Fog.On:
                            return "Enables vertex-based fog.";
                    }
                    break;

                case ParticleMethods.Frame_Blend:
                    switch ((Frame_Blend)optionIndex)
                    {
                        case Frame_Blend.Off:
                            return "No frame blending.";
                        case Frame_Blend.On:
                            return "Blend 2 frames using different texture sampling coordinates.";
                    }
                    break;

                case ParticleMethods.Self_Illumination:
                    switch ((Self_Illumination)optionIndex)
                    {
                        case Self_Illumination.None:
                            return "No self illumination.";
                        case Self_Illumination.Constant_Color:
                            return "Enables vertex-based constant color self illumination.";
                    }
                    break;
            }
            return "No description available.";
        }

        public override IShaderGenerator BuildShaderGenerator(List<RenderMethodOptionIndex> shaderOptions)
        {
            var albedo = (Albedo)shaderOptions[0].OptionIndex;
            var blend_mode = (Blend_Mode)shaderOptions[1].OptionIndex;
            var specialized_rendering = (Specialized_Rendering)shaderOptions[2].OptionIndex;
            var lighting = (Lighting)shaderOptions[3].OptionIndex;
            var render_target = (Render_Targets)shaderOptions[4].OptionIndex;
            var depth_fade = (Depth_Fade)shaderOptions[5].OptionIndex;
            var black_point = (Black_Point)shaderOptions[6].OptionIndex;
            var fog = (Fog)shaderOptions[7].OptionIndex;
            var frame_blend = (Frame_Blend)shaderOptions[8].OptionIndex;
            var self_illumination = (Self_Illumination)shaderOptions[9].OptionIndex;
           
             
            return new ParticleGenerator(albedo, blend_mode, specialized_rendering, lighting, render_target, depth_fade, black_point, fog, frame_blend, self_illumination);
        }
    }
}
