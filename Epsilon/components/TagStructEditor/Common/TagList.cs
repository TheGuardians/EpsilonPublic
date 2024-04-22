using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagStructEditor.Common
{
    public class TagList
    {
        public ObservableCollection<TagGroupItem> Groups { get; set; }
        public ObservableCollection<TagInstanceItem> Instances { get; set; }
        public ObservableCollection<Tag> GroupTags { get; set; }

        public TagList(GameCache cache)
        {
            Build(cache);
        }

        private void Build(GameCache cache)
        {
            Instances = new ObservableCollection<TagInstanceItem>(
                cache.TagCache
                .NonNull()
                .Select(tag => new TagInstanceItem(tag))
                .OrderBy(item => item.Name));

            Groups = new ObservableCollection<TagGroupItem>(
                Instances
                .GroupBy(item => item.Instance.Group)
                .Select(group => new TagGroupItem(group.Key, group))
                .OrderBy(item => item.TagAscii));

            var tags = new HashSet<Tag>();
            foreach (var instance in Instances)
            {
                tags.Add(instance.Instance.Group.Tag);
                tags.Add(instance.Instance.Group.ParentTag);
                tags.Add(instance.Instance.Group.GrandParentTag);
            }
            GroupTags = new ObservableCollection<Tag>(tags.OrderBy(x => x.Value));
        }
    }

    public class TagGroupItem
    {
        public TagGroup Group { get; set; }
        public string TagAscii { get; set; }

        public ObservableCollection<TagInstanceItem> Instances { get; set; }

        public TagGroupItem(TagGroup group, IEnumerable<TagInstanceItem> items)
        {
            Group = group;
            TagAscii = group.Tag.ToString();
            Instances = new ObservableCollection<TagInstanceItem>(items);
        }
    }

    public class TagInstanceItem
    {
        public string Name { get; set; }
        public CachedTag Instance { get; set; }

        public TagInstanceItem(CachedTag instance)
        {
            Name = instance.Name ?? instance.ToString();
            Instance = instance;
        }
    }
}
