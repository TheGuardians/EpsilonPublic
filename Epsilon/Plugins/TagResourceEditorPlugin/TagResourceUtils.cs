using System;
using TagTool.Cache;
using TagTool.Cache.Resources;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagResourceEditorPlugin
{
    class TagResourceUtils
    {
        // TODO: move some of this back out to tagtool?

        public static object GetResourceDefinition(ResourceCache cache, Type definitionType, TagResourceReference resourceReference)
        {
            return cache.GetResourceDefinition(resourceReference, definitionType);
        }

        public static Type GetTagResourceDefinitionType(GameCache cache, TagResourceReference resourceReference)
        {
            if(cache is GameCacheGen3 gen3Cache)
            {
                var resource = gen3Cache.ResourceCacheGen3.GetTagResourceFromReference(resourceReference);
                var gestalt = gen3Cache.ResourceCacheGen3.ResourceGestalt;
                if (resource.ResourceTypeIndex < 0 || resource.ResourceTypeIndex >= gestalt.ResourceDefinitions.Count)
                    return null;

                var typeDef = gestalt.ResourceDefinitions[resource.ResourceTypeIndex];
                var typeName = cache.StringTable.GetString(typeDef.Name);
                return GetTagResourceDefinitionTypeGen3(typeName);
            }
            else if (cache is GameCacheGen4 gen4Cache)
            {
                var resource = gen4Cache.ResourceCacheGen4.GetTagResourceFromReference(resourceReference);
                var gestalt = gen4Cache.ResourceCacheGen4.ResourceGestalt;
                if (resource.ResourceTypeIndex < 0 || resource.ResourceTypeIndex >= gestalt.ResourceTypeIdentifiers.Count)
                    return null;

                var typeDef = gestalt.ResourceTypeIdentifiers[resource.ResourceTypeIndex];
                var typeName = cache.StringTable.GetString(typeDef.Name);
                return GetTagResourceDefinitionTypeGen4(typeName);
            }
            else if(cache is GameCacheHaloOnlineBase)
            {
                if (resourceReference.HaloOnlinePageableResource == null)
                    return null;

                return resourceReference.HaloOnlinePageableResource.GetDefinitionType();
            }

            return null;
        }

        private static Type GetTagResourceDefinitionTypeGen3(string name)
        {
            switch(name)
            {
                case "model_animation_tag_resource":
                    return typeof(ModelAnimationTagResource);
                case "bink_resource":
                    return typeof(ModelAnimationTagResource);
                case "bitmap_texture_interop_resource":
                    return typeof(BitmapTextureInteropResource);
                case "bitmap_texture_interleaved_interop_resource":
                    return typeof(BitmapTextureInterleavedInteropResource);
                case "structure_bsp_tag_resources":
                    return typeof(StructureBspTagResources);
                case "structure_bsp_cache_file_tag_resources":
                    return typeof(StructureBspCacheFileTagResources);
                case "sound_resource_definition":
                    return typeof(SoundResourceDefinition);
                case "render_geometry_api_resource_definition":
                    return typeof(RenderGeometryApiResourceDefinition);
                default:
                    return null;
            }
        }

        private static Type GetTagResourceDefinitionTypeGen4(string name)
        {
            switch (name)
            {
                case "collision_model_resource":
                    return typeof(TagTool.Tags.Resources.Gen4.CollisionModelResource);
                case "model_animation_tag_resource":
                    return typeof(TagTool.Tags.Resources.Gen4.ModelAnimationTagResource);
                case "bink_resource":
                    return typeof(TagTool.Tags.Resources.Gen4.ModelAnimationTagResource);
                case "bitmap_texture_interop_resource":
                    return typeof(TagTool.Tags.Resources.Gen4.BitmapTextureInteropResource);
                case "bitmap_texture_interleaved_interop_resource":
                    return typeof(TagTool.Tags.Resources.Gen4.BitmapTextureInterleavedInteropResource);
                //case "structure_bsp_tag_resources":
                //    return typeof(TagTool.Tags.Resources.Gen4.StructureBspTagResources);
                //case "structure_bsp_cache_file_tag_resources":
                //    return typeof(TagTool.Tags.Resources.Gen4.StructureBspCacheFileTagResources);
                case "sound_resource_definition":
                    return typeof(TagTool.Tags.Resources.Gen4.SoundResourceDefinition);
                case "render_geometry_api_resource_definition":
                    return typeof(TagTool.Tags.Resources.Gen4.RenderGeometryApiResourceDefinition);
                default:
                    return null;
            }
        }
    }
}
