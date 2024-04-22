using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TagStructEditor.Common;
using TagStructEditor.Fields;

namespace TagStructEditor.Controls
{
    public class StructEditorControl : Control
    {
        public IField Field
        {
            get { return (IField)GetValue(FieldProperty); }
            set { SetValue(FieldProperty, value); }
        }

        public bool IsVirtualizing
        {
            get { return (bool)GetValue(IsVirtualizingProperty); }
            set { SetValue(IsVirtualizingProperty, value); }
        }

        public static readonly DependencyProperty IsVirtualizingProperty =
            DependencyProperty.Register("IsVirtualizing",
                typeof(bool),
                typeof(StructEditorControl),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty FieldProperty =
            DependencyProperty.Register("Field", typeof(IField), typeof(StructEditorControl), new PropertyMetadata(null));


        public IField FocusedField
        {
            get { return (IField)GetValue(FocusedFieldProperty); }
            set { SetValue(FocusedFieldProperty, value); }
        }

        public static readonly DependencyProperty FocusedFieldProperty =
            DependencyProperty.Register("FocusedField", typeof(IField), typeof(StructEditorControl), new FrameworkPropertyMetadata(OnFocusedFieldChanged));

        private static void OnFocusedFieldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IField f)
                (d as StructEditorControl).BringFieldIntoView(f);
        }

        static StructEditorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StructEditorControl), new FrameworkPropertyMetadata(typeof(StructEditorControl)));
        }


        private void BringFieldIntoView(IField field)
        {
            ScrollTo(field);
        }

        public void ScrollTo(IField field)
        {
            // This is an absolutely awful hack. The problem is that the structure editor has nested listboxes and we 
            // have no idea which listbox the element is in. We also don't know if the listbox has even been created yet-
            // in the case of virtualization - which complicates things further. 
            // In hindsight, the tag field tree should probably have been flattened. This would not only make this horrible hack below go away, 
            // but i suspect it'd greatly improve performance. However, it brings its own complications in other areas such as width calculation, 
            // maintaining parent-child state for expandables (you would need to fake containment)
            // It's something to think about anyway.

            // walk up the tree collecting ancestors
            var stack = new Stack<IField>();

            while (field.Parent != null)
            {
                if (field is IExpandable expandable)
                    expandable.IsExpanded = true;

                stack.Push(field);
                field = field.Parent;
            }

            BruteforceScrollIntoView(this, stack.AsEnumerable());
        }


        public void BruteforceScrollIntoView(DependencyObject root, IEnumerable<IField> targetFields)
        {
            // 1. Walk the visual tree. 
            // 2. For each ListBox enounctered, check if it contains a container for the next target tag field
            // 3. call ScrollIntoView() this will create an item container for the item if it doesn't exist (hasn't been scrolled to before)
            // 4. if after the call to ScrollIntoView() there is not item container, we know the item is not in that listbox
            // 5. If the item isnt't in the listbox ignore it. If it is, recurse through that item container's visual tree looking for the remaining target tag fields

            if (root == null || !targetFields.Any())
                return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child is ListBox listbox)
                {
                    var item = targetFields.First();

                    listbox.ScrollIntoView(item);

                    var container = listbox.ItemContainerGenerator.ContainerFromItem(item);
                    if (container != null)
                    {
                        targetFields = targetFields.Skip(1);
                        BruteforceScrollIntoView(container, targetFields.Skip(1));
                    }
                }

                BruteforceScrollIntoView(child, targetFields);
            }
        }
    }
}
