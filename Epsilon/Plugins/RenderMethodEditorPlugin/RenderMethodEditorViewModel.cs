using CacheEditor.TagEditing;
using CacheEditor.TagEditing.Messages;
using HaloShaderGenerator.Generator;
using RenderMethodEditorPlugin.ShaderMethods;
using RenderMethodEditorPlugin.ShaderMethods.Particle;
using RenderMethodEditorPlugin.ShaderMethods.Shader;
using RenderMethodEditorPlugin.ShaderMethods.Halogram;
using RenderMethodEditorPlugin.ShaderMethods.Decal;
using RenderMethodEditorPlugin.ShaderMethods.Screen;
using RenderMethodEditorPlugin.ShaderParameters;
using System.Linq;
using System.Collections.ObjectModel;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using CacheEditor;

namespace RenderMethodEditorPlugin
{
    class RenderMethodEditorViewModel : TagEditorPluginBase
    {
        private RenderMethod _renderMethod;
        private GameCache _cache;
        private RenderMethodTemplate _renderMethodTemplate;
        private RenderMethodDefinition _renderMethodDefinition;
        private RenderMethod.RenderMethodPostprocessBlock _shaderProperty;

        public ObservableCollection<BooleanConstant> BooleanConstants { get; private set;  } = new ObservableCollection<BooleanConstant>();
        public ObservableCollection<Method> ShaderMethods { get; private set; } = new ObservableCollection<Method>();
        public ObservableCollection<GenericShaderParameter> ShaderParameters { get; private set; } = new ObservableCollection<GenericShaderParameter>();

        public RenderMethodEditorViewModel(GameCache cache, RenderMethod renderMethod)
        {
            _cache = cache;
            Load(cache, renderMethod);
        }

        private void Load(GameCache cache, RenderMethod renderMethod)
        {
            _renderMethod = renderMethod;
            _shaderProperty = _renderMethod.ShaderProperties[0];
            var templateName = _shaderProperty.Template.Name;

            if (templateName == null)
                return;

            var templateTypeSplitIndex = 1;
            if (templateName.Contains("ms30"))
                templateTypeSplitIndex = 2;

            string templateType = templateName.Split('\\')[templateTypeSplitIndex].Split('_')[0];

            var options = _renderMethod.Options.ConvertAll(x => (byte)x.OptionIndex);

            System.Collections.Generic.List<RenderMethodOption.ParameterBlock> rmopParameters;
            using (var stream = _cache.OpenCacheRead())
            {
                _renderMethodTemplate = cache.Deserialize<RenderMethodTemplate>(stream, _shaderProperty.Template);
                _renderMethodDefinition = cache.Deserialize<RenderMethodDefinition>(stream, renderMethod.BaseRenderMethod);
                // get parameters, auto macro and global parameters are ignored for now
                rmopParameters = TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.GatherParameters(cache, stream, _renderMethodDefinition, options, false);
            }

            ShaderMethods = new ObservableCollection<Method>();
            for (int i = 0; i < _renderMethodDefinition.Categories.Count; i++)
            {
                short optionIndex = 0; // assume an index of 0 for outdated tags
                
                if (i < _renderMethod.Options.Count)
                    optionIndex = _renderMethod.Options[i].OptionIndex;

                if (optionIndex < _renderMethodDefinition.Categories[i].ShaderOptions.Count)
                {
                    string categoryName = cache.StringTable.GetString(_renderMethodDefinition.Categories[i].Name);
                    string optionName = cache.StringTable.GetString(_renderMethodDefinition.Categories[i].ShaderOptions[optionIndex].Name);

                    ShaderMethods.Add(new Method(categoryName, optionName, "Description N/A", i, optionIndex));
                }
            }

            foreach (var parameter in rmopParameters)
            {
                var name = cache.StringTable.GetString(parameter.Name);

                switch (parameter.Type)
                {
                    case RenderMethodOption.ParameterBlock.OptionDataType.Real:
                        for (int i = 0; i < _renderMethodTemplate.RealParameterNames.Count; i++)
                        {
                            if (cache.StringTable.GetString(_renderMethodTemplate.RealParameterNames[i].Name) == name)
                            {
                                string description = $"1D vector for parameter \"{name}\"";
                                ShaderParameters.Add(new FloatShaderParameter(_renderMethod.ShaderProperties[0], name, description, i));
                                break;
                            }
                        }
                        break;
                    case RenderMethodOption.ParameterBlock.OptionDataType.Color:
                    case RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor:
                        for (int i = 0; i < _renderMethodTemplate.RealParameterNames.Count; i++)
                        {
                            if (cache.StringTable.GetString(_renderMethodTemplate.RealParameterNames[i].Name) == name)
                            {
                                string description = $"4D vector for parameter \"{name}\"";
                                ShaderParameters.Add(new Float4ShaderParameter(_renderMethod.ShaderProperties[0], name, description, i));
                                break;
                            }
                        }
                        break;
                    case RenderMethodOption.ParameterBlock.OptionDataType.Bitmap:
                        for (int i = 0; i < _renderMethodTemplate.TextureParameterNames.Count; i++)
                        {
                            if (cache.StringTable.GetString(_renderMethodTemplate.TextureParameterNames[i].Name) == name)
                            {
                                string description = $"Bitmap tag for texture \"{name}\"";
                                ShaderParameters.Add(new SamplerShaderParameter(_renderMethod.ShaderProperties[0], name, description, i));
                                break;
                            }
                        }
                        break;
                    //case RenderMethodOption.ParameterBlock.OptionDataType.Int:
                    //    for (int i = 0; i < _renderMethodTemplate.IntegerParameterNames.Count; i++)
                    //    {
                    //        if (cache.StringTable.GetString(_renderMethodTemplate.IntegerParameterNames[i].Name) == name)
                    //        {
                    //            string description = $"Whole integer for parameter \"{name}\"";
                    //            ShaderParameters.Add(new IntegerShaderParameter(_renderMethod.ShaderProperties[0], name, description, i));
                    //            break;
                    //        }
                    //    }
                    //    break;
                    case RenderMethodOption.ParameterBlock.OptionDataType.Bool:
                        for (int i = 0; i < _renderMethodTemplate.BooleanParameterNames.Count; i++)
                        {
                            if (cache.StringTable.GetString(_renderMethodTemplate.BooleanParameterNames[i].Name) == name)
                            {
                                string description = $"Checkbox for on/off parameter \"{name}\"";
                                ShaderParameters.Add(new BooleanShaderParameter(_renderMethod.ShaderProperties[0], name, description, i, cache, _renderMethodTemplate.BooleanParameterNames));
                                break;
                            }
                        }
                        break;
                }

                // xform
                if (parameter.Type == RenderMethodOption.ParameterBlock.OptionDataType.Bitmap)
                {
                    for (int i = 0; i < _renderMethodTemplate.RealParameterNames.Count; i++)
                    {
                        if (cache.StringTable.GetString(_renderMethodTemplate.RealParameterNames[i].Name) == name)
                        {
                            string description = $"Transform vector for texture \"{name}\"";
                            ShaderParameters.Add(new TransformShaderParameter(_renderMethod.ShaderProperties[0], name, description, i));
                            break;
                        }
                    }
                }
            }

            if (_renderMethodDefinition.Flags.HasFlag(RenderMethodDefinition.RenderMethodDefinitionFlags.UseAutomaticMacros))
            {
                for (int i = 0; i < _renderMethodDefinition.Categories.Count; i++)
                {
                    string name = cache.StringTable.GetString(_renderMethodDefinition.Categories[i].Name);

                    if (TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.AutoMacroIsParameter(name,
                        (HaloShaderGenerator.Globals.ShaderType)System.Enum.Parse(typeof(HaloShaderGenerator.Globals.ShaderType), templateType, true)))
                    {
                        for (int j = 0; j < _renderMethodTemplate.RealParameterNames.Count; j++)
                        {
                            if (cache.StringTable.GetString(_renderMethodTemplate.RealParameterNames[j].Name) == name)
                            {
                                string description = $"This value should match the option index for category \"{name}\"";
                                ShaderParameters.Add(new CategoryShaderParameter(_renderMethod.ShaderProperties[0], "category_" + name, description, j));
                                break;
                            }
                        }
                    }
                }
            }

            this.NotifyOfPropertyChange(nameof(ShaderMethods));
            this.NotifyOfPropertyChange(nameof(ShaderParameters));
        }

        // get boolean values from existing tag data

        private void ParseBooleanArguments()
        {
            for(int i = 0; i < _renderMethodTemplate.BooleanParameterNames.Count; i++)
            {
                string name = _cache.StringTable.GetString(_renderMethodTemplate.BooleanParameterNames[i].Name);
                BooleanConstants.Add(new BooleanConstant(_shaderProperty, name, FindDescriptionFromName(name), i));
            }
        }

        private string FindDescriptionFromName(string shaderArgName)
        {
            if(ShaderArgumentsDescription.ArgsDescription.ContainsKey(shaderArgName))
                return ShaderArgumentsDescription.ArgsDescription[shaderArgName];
            else
                return "Missing description";
        }

        public void Test()
        {
            PostMessage(this, new DefinitionDataChangedEvent(_renderMethod));
        }

        protected override void OnMessage(object sender, object message)
        {
            if (message is DefinitionDataChangedEvent e)
            {
               Load(_cache, (RenderMethod)e.NewData);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            _renderMethod = null;
            _cache = null;
            _renderMethodTemplate = null;
            _renderMethodDefinition = null;
            _shaderProperty = null;
            BooleanConstants.Clear();
            ShaderMethods.Clear();
            ShaderParameters.Clear();
        }
    }

    class ShaderConstant
    {
        public RenderMethod.RenderMethodPostprocessBlock Property;
        public string Name { get; set; }
        public string Description { get; set; }
        public int TemplateIndex;
        public ShaderConstant(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex)
        {
            Name = ShaderStringConverter.ToPrettyFormat(name);
            TemplateIndex = templateIndex;
            Property = property;
            Description = desc;
        }
    }

    class BooleanConstant : ShaderConstant
    {
        public bool Value
        {
            get => (((int)(Property.BooleanConstants) >> TemplateIndex) & 1) == 1;
            set
            {
                if(value == true)
                    Property.BooleanConstants = (uint)((int)Property.BooleanConstants | (1 << TemplateIndex));
                else
                    Property.BooleanConstants = (uint)((int)Property.BooleanConstants & ~(1 << TemplateIndex));
            }
        }

        public BooleanConstant(RenderMethod.RenderMethodPostprocessBlock property, string name, string desc, int templateIndex) : base(property, name, desc, templateIndex)
        {
        }
    }
}
