using System.Collections;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;

namespace TagResourceEditorPlugin
{
    class ResourceReferenceCollector
    {
        private readonly GameCache _cache;
        private readonly List<TagResourceReference> _resourceReferences = new List<TagResourceReference>();

        public ResourceReferenceCollector(GameCache cache, TagStructure tagStructure)
        {
            _cache = cache;
            VisitTagStructure(tagStructure);
        }

        public static IEnumerable<TagResourceReference> Collect(GameCache cache, TagStructure tagStructure)
        {
            return new ResourceReferenceCollector(cache, tagStructure)._resourceReferences;
        }

        private void VisitData(object data)
        {
            switch (data)
            {
                case TagResourceReference resourceReference:
                    VisitPageableResource(resourceReference);
                    break;

                case TagStructure tagStructure:
                    VisitTagStructure(tagStructure);
                    break;
                case IList collection:
                    VisitCollection(collection);
                    break;
            }
        }

        private void VisitCollection(IList collection)
        {
            foreach (var element in collection)
                VisitData(element);
        }

        private void VisitPageableResource(TagResourceReference resourceReference)
        {
            _resourceReferences.Add(resourceReference);
        }

        private void VisitTagStructure(TagStructure tagStructure)
        {
            foreach (var field in tagStructure.GetTagFieldEnumerable(_cache.Version, _cache.Platform))
            {
                var data = field.GetValue(tagStructure);
                VisitData(data);
            }
        }
    }
}
