using EpsilonLib.Shell.TreeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags;

namespace CacheEditor.Components.TagTree
{
    class TagTreeGroupView : ITagTreeViewMode
    {
        private readonly TagTreeGroupDisplayMode DisplayMode;
        private readonly bool ShowAltText;

        public TagTreeGroupView(TagTreeGroupDisplayMode displayMode, bool showGroupAltText)
        {
            DisplayMode = displayMode;
            ShowAltText = showGroupAltText;
        }

        public IEnumerable<ITreeNode> BuildTree(GameCache cache, Func<CachedTag, bool> filter)
        {
           return cache.TagCache
                .NonNull()
             .Where(filter)
             .GroupBy(tag => tag.Group, CreateTagNode)
             .Select(group => CreateGroupNode(cache, group))
             .OrderBy(node => node.Text);
        }

        private TagTreeTagNode CreateTagNode(CachedTag tag)
        {
            return new TagTreeTagNode(tag, () => FormatName(tag));
        }

        private TagTreeGroupNode CreateGroupNode(GameCache cache, IGrouping<TagGroup, TagTreeNode> group)
        {
            var groupNode = new TagTreeGroupNode()
            {
                Text = FormatGroupNodeName(group.Key),
                AltText = FormatGroupAltText(group.Key),
                Tag = group.Key,
                Children = new ObservableCollection<ITreeNode>(group.OrderBy(node => node.Text))
            };
            foreach (var node in group)
                node.Parent = groupNode;

            return groupNode;
        }

        private string FormatGroupNodeName(TagGroup group)
        {
            if (DisplayMode == TagTreeGroupDisplayMode.TagGroupName)
                return group.ToString();
            else if (DisplayMode == TagTreeGroupDisplayMode.TagGroup)
                return group.Tag.ToString();
            else
                return group.ToString();
        }

        private string FormatGroupAltText(TagGroup group)
        {
            if(ShowAltText)
            {
                if (DisplayMode == TagTreeGroupDisplayMode.TagGroupName)
                    return group.Tag.ToString();
                else
                    return group.ToString();
            }

            return null;
        }

        private string FormatName(CachedTag tag)
        {
            if(tag.Name == null)
                return $"0x{tag.Index:X8}";
            else
                return tag.Name; 
        }
    }

    class TagTreeGroupNode : TagTreeNode
    {

    }
}
