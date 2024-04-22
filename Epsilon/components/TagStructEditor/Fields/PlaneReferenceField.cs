using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Geometry.BspCollisionGeometry;
using static TagTool.Tags.Definitions.RenderMethodTemplate;

namespace TagStructEditor.Fields
{
    public class PlaneReferenceField : ValueField
    {
        public int TriangleIndex { get; set; }
        public int ClusterIndex{ get; set; }

        public PlaneReferenceField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            return;
        }

        protected override void OnPopulate(object value)
        {
            var planeRef = (StructureSurfaceToTriangleMapping)value;
            TriangleIndex = planeRef.TriangleIndex;
            ClusterIndex = planeRef.ClusterIndex;
        }

        protected void OnTriangleIndexChanged() => UpdateValue();
        protected void OnClusterIndexChanged() => UpdateValue();

        void UpdateValue() => SetActualValue(new StructureSurfaceToTriangleMapping(TriangleIndex, ClusterIndex));
    }
}
