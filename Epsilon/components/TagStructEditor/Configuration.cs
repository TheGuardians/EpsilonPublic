using System;
using TagStructEditor.Fields;
using TagTool.Cache;

namespace TagStructEditor
{
    public class Configuration
    {
        /// <summary>
        /// Callback function for whenever the value of a field changes
        /// </summary>
        public Action<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        ///  Callback function to run a tag selection dialog
        /// </summary>
        public Func<CachedTag> BrowseTag;

        /// <summary>
        /// Callback function to open a tag
        /// </summary>
        public Action<CachedTag> OpenTag;

        public bool DisplayFieldTypes { get; set; }
        public bool DisplayFieldOffsets { get; set; }
        public bool CollapseBlocks { get; set; }
    }
}
