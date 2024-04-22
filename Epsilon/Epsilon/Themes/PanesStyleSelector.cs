using Shared;
using Stylet;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp20
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style DocumentStyle
        {
            get;
            set;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
           if (item is ITool)
               return ToolStyle;
           
           if (item is IScreen)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
