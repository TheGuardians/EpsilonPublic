using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin.ShaderParameters
{
    class GenericShaderParameter
    {
        public RenderMethod.RenderMethodPostprocessBlock Property;
        public string Name { get; set; }
        public string PrettyName { get; set; }
        public string Description { get; set; }
        public int TemplateIndex;

        public GenericShaderParameter(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex)
        {
            Name = name;
            PrettyName = ShaderStringConverter.ToPrettyFormat(name);
            TemplateIndex = templateIndex;
            Property = property;
            Description = desc ?? "No description available";
        }
    }
}
