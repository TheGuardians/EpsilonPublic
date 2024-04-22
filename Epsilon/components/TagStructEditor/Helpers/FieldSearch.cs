using System;
using System.Collections.Generic;
using System.Linq;
using TagStructEditor.Fields;

namespace TagStructEditor.Helpers
{
    /// <summary>
    /// Helper class for searching for fields
    /// </summary>
    public class FieldSearch
    {
        public static IReadOnlyList<ValueField> Search(IField root, string query)
        {
            return new TagFieldSearchVisitor(query).Search(root);
        }

        private class TagFieldSearchVisitor : FieldVisitorBase
        {
            public string Query { get; set; }
            private List<ValueField> Results { get; }

            public TagFieldSearchVisitor(string query)
            {
                Query = query;
                Results = new List<ValueField>();
            }

            public IReadOnlyList<ValueField> Search(IField field)
            {
                field.Accept(this);
                return Results;
            }

            public override void Visit(BlockField field)
            {
                ProcessValueField(field);

                if(field.Name == "Functions")
                {

                }

                base.Visit(field);
            }

            public override void Visit(InlineStructField field)
            {
                ProcessValueField(field);

                base.Visit(field);
            }

            public override void Visit(RealVector3dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(CachedTagField field)
            {
                if (!ProcessValueField(field))
                {
                    bool nameMatches = field.SelectedInstance != null && field.SelectedInstance.Name.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1;
                    bool groupMatches = field.SelectedGroup != null && field.SelectedGroup.TagAscii.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1;

                    if (nameMatches || groupMatches)
                    {
                        Results.Add(field);
                        return;
                    }
                }
            }

            public override void Visit(Point2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(DatumHandleField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(StringField field)
            {
                if (field.Value.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    Results.Add(field);
                    return;
                }

                ProcessValueField(field);
            }

            public override void Visit(StringIdField field)
            {
                if (field.Value.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    Results.Add(field);
                    return;
                }

                ProcessValueField(field);
            }

            public override void Visit(Rectangle2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(ArgbColorField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealRectangle3dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealRgbColorField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealArgbColorField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealRgbaColorField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealVector2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealEulerAngles2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(DataField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(BoolField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealQuaternionField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(AngleField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(GroupTagField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(CacheAddressField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealEulerAngles3dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Int8Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealPoint3dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealPlane2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(RealPlane3dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(UInt8Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(FlagsField field)
            {
                if (!ProcessValueField(field))
                {
                    bool isMatch = field.Flags.Any(member => member.Name.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1);
                    if (isMatch)
                    {
                        Results.Add(field);
                    }
                }
            }

            public override void Visit(EnumField field)
            {
                if (!ProcessValueField(field))
                {
                    bool isMatch = field.Values.Any(member => member.Name.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1);
                    if (isMatch)
                    {
                        Results.Add(field);
                    }
                }
            }

            public override void Visit(RealPoint2dField field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Int16Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(UInt16Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Int32Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(UInt32Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Int64Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(UInt64Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Float32Field field)
            {
                ProcessValueField(field);
            }

            public override void Visit(Float64Field field)
            {
                ProcessValueField(field);
            }

            private bool ProcessValueField(ValueField field)
            {
                var isMatch = field.Name.IndexOf(Query, StringComparison.OrdinalIgnoreCase) != -1;

                if (isMatch)
                    Results.Add(field);

                return isMatch;
            }
        }
    }
}
