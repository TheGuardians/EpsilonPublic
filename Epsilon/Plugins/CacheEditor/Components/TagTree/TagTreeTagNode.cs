using EpsilonLib.Themes;
using System;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;

namespace CacheEditor.Components.TagTree
{
    public class TagTreeTagNode : TagTreeNode 
    {
        private Func<string> _textDelegate;

        public TagTreeTagNode(CachedTag tag, Func<string> textDelegate)
        {
            Tag = tag;
            _textDelegate = textDelegate;

            UpdateAppearance();
        }

        public void UpdateName()
        {
            Text = _textDelegate();
        }

        public override void UpdateAppearance()
        {
            UpdateName();
            TextColor = DetermineTextColor();
        }

        private ColorHint DetermineTextColor()
        {
            if (Tag is CachedTagHaloOnline hoTag)
                if (hoTag.IsEmpty())
                    return ColorHint.Muted;
            return ColorHint.Default;
        }
    }
}
