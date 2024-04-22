using CacheEditor;
using Shared;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace BlamScriptEditorPlugin
{
    [Export(typeof(ITagEditorPluginProvider))]
    class ScriptTagEditorPluginProvider : ITagEditorPluginProvider
    {
        private readonly Lazy<IShell> _shell;

        [ImportingConstructor]
        public ScriptTagEditorPluginProvider(Lazy<IShell> shell)
        {
            _shell = shell;
        }

        public string DisplayName => "Scripts";

        public int SortOrder => 2;

        public async Task<ITagEditorPlugin> CreateAsync(TagEditorContext context)
        {
            var definition = await context.DefinitionData as Scenario;
            var vm = new ScriptTagEditorViewModel(_shell.Value, context.CacheEditor.CacheFile, definition);
            Task.Run(async () => await vm.LoadAsync());
            return vm;
        }

        public bool ValidForTag(ICacheFile cache, CachedTag tag)
        {
            return tag.IsInGroup("scnr");
        }
    }
}
