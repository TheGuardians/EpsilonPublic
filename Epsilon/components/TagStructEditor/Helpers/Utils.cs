using System;
using System.Collections;
using System.Text;
using TagTool.Tags;

namespace TagStructEditor.Helpers
{
    public class Utils
    {
        public static string DemangleName(string name)
        {
            var buff = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsLower(name[i - 1]) && char.IsUpper(name[i]))
                {
                    buff.Append(' ');
                    buff.Append(name[i]);
                }
                else
                {
                    if (name[i] == '.')
                        buff.Append(' ');
                    else if (name[i] == '[')
                    {
                        buff.Append(' ');
                        buff.Append(name[i]);
                    }
                    else
                        buff.Append(name[i]);
                }
            }

            return buff.ToString();
        }


        public static object ActivateType(Type elementType)
        {
            if (elementType.IsAbstract)
                return null;

            if (elementType.IsArray)
                return null;

            var instance = Activator.CreateInstance(elementType);

            if (!Attribute.IsDefined(elementType, typeof(TagStructureAttribute)) || !elementType.IsSubclassOf(typeof(TagStructure)))
                return instance;

            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(elementType, TagTool.Cache.CacheVersion.Unknown, TagTool.Cache.CachePlatform.Original))
            {
                var fieldType = tagFieldInfo.FieldType;

                if (fieldType.IsArray && tagFieldInfo.Attribute.Length > 0)
                {
                    var array = (IList)Activator.CreateInstance(tagFieldInfo.FieldType,
                        new object[] { tagFieldInfo.Attribute.Length });

                    for (var i = 0; i < tagFieldInfo.Attribute.Length; i++)
                        array[i] = ActivateType(fieldType.GetElementType());

                    tagFieldInfo.SetValue(instance, array);
                }
                else
                {
                    var constructor = tagFieldInfo.FieldType.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        tagFieldInfo.SetValue(instance, ActivateType(tagFieldInfo.FieldType));
                    }
                    else
                    {
                        if (tagFieldInfo.FieldType.IsValueType)
                            tagFieldInfo.SetValue(instance, Activator.CreateInstance(tagFieldInfo.FieldType));
                        else
                            tagFieldInfo.SetValue(instance, null);
                    }
                }
            }

            return instance;
        }
    }
}
