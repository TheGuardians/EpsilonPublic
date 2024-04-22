using System;
using TagTool.Geometry.BspCollisionGeometry;
using static TagTool.Geometry.BspCollisionGeometry.Bsp3dNode;

namespace TagStructEditor.Fields
{
    public class Bsp3dNodeField : ValueField
    {
        public Bsp3dNodeField(ValueFieldInfo info) : base(info)
        {
        }

        public static readonly Type ChildTypeEnum = typeof(ChildType);

        public ulong Value { get; set; }
        public int Plane { get; set; }
        public ChildType BackChildType { get; set; }
        public int BackChildIndex { get; set; }
        public ChildType FrontChildType { get; set; }
        public int FrontChildIndex { get; set; }

        public override void Accept(IFieldVisitor visitor)
        {
            //throw new NotImplementedException();
        }

        protected override void OnPopulate(object value)
        {
            var node = (Bsp3dNode)value;
            Plane = node.Plane;
            FrontChildIndex = Bsp3dNode.GetChildIndex(node.FrontChild);
            FrontChildType = Bsp3dNode.GetChildType(node.FrontChild);
            BackChildIndex = Bsp3dNode.GetChildIndex(node.BackChild);
            BackChildType = Bsp3dNode.GetChildType(node.BackChild);
        }

        protected void OnValueChanged() => UpdateValue();
        protected void OnPlaneChanged() => UpdateValue();
        protected void OnBackChildIndexChanged() => UpdateValue();
        protected void OnFrontChildIndexChanged() => UpdateValue();
        protected void OnBackChildTypeChanged() => UpdateValue();
        protected void OnFrontChildTypeChanged() => UpdateValue();

        private void UpdateValue()
        {
            var node = new Bsp3dNode();
            node.Plane = Plane;
            node.BackChild = Bsp3dNode.CreateChild(BackChildType, BackChildIndex);
            node.FrontChild = Bsp3dNode.CreateChild(FrontChildType, FrontChildIndex);
            Value = node.Value;
            SetActualValue(node);
        }
    }
}
