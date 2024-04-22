using Stylet;
using System.Windows;
using System.Windows.Controls;

namespace CacheEditor
{
    class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }
        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ICacheEditorTool)
                return ToolStyle;
            if (item is IScreen)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
