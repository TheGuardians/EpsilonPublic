using System.Collections.Generic;

namespace RenderMethodEditorPlugin
{
    static class ShaderArgumentsDescription
    {
        public static readonly Dictionary<string, string> ArgsDescription = new Dictionary<string, string>
        {
            {"order3_area_specular", "Use order 3 SH coefficients for area specular calculations." },
            {"use_material_texture", "Use material texture to define per-pixel values for specular coefficient, albedo blend, environment map specular and roughness." },
            {"no_dynamic_lights", "Deactivate the contribution of dynamic light sources." }
        };
    }
}
