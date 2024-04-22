using TagStructEditor.Common;
using TagStructEditor.Fields;

namespace TagStructEditor.Helpers
{
    /// <summary>
    /// Helper class to recursively expand tag fields
    /// </summary>
    public class FieldExpander
    {
        public enum ExpandTarget
        {
            All,
            TopLevel
        }

        public enum ExpandMode
        {
            Collapse,
            Expand
        }

        public static void Expand(IField root, ExpandTarget target, ExpandMode mode)
        {
            root.Accept(new ExpanderVisitor(root, target, mode));
        }

        private class ExpanderVisitor : FieldVisitorBase
        {
            public ExpandTarget Target { get; set; }
            public ExpandMode Mode { get; set; }
            public IField Root { get; set; }

            public ExpanderVisitor(IField root, ExpandTarget target, ExpandMode mode)
            {
                Root = root;
                Target = target;
                Mode = mode;
            }

            public override void Visit(DataField field)
            {
                Expand(field);
                base.Visit(field);
            }

            public override void Visit(InlineStructField field)
            {
                Expand(field);
                base.Visit(field);
            }

            public override void Visit(BlockField field)
            {
                Expand(field);
                base.Visit(field);
            }

            private void Expand(IExpandable expandable)
            {
                var field = (IField)expandable;
                if (Target == ExpandTarget.TopLevel && field.Parent != Root)
                    return;

                expandable.IsExpanded = Mode == ExpandMode.Expand;
            }
        }
    }
}
