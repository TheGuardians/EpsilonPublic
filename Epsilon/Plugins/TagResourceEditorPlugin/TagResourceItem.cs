using System;
using TagTool.Tags;

namespace TagResourceEditorPlugin
{
    class TagResourceItem
    {
        public string DisplayName { get; set; }
        public TagResourceReference Resource { get; set; }
        public Type DefinitionType { get; set; }
    }
}
