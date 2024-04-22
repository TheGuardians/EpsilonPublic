using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EpsilonLib.Utils
{
    public static class VisualTreeHelpers
    {
        public static IEnumerable<T> FindAncestors<T>(this Visual visual)
        {
            while (visual != null)
            {
                if (visual is T value)
                    yield return value;

                visual = VisualTreeHelper.GetParent(visual) as Visual;
            }
        }

        public static T FindAncestorDataContext<T>(UIElement source)
        {
            while (source != null)
            {
                if (source is FrameworkElement fe && fe.DataContext is T data)
                    return data;

                source = (UIElement)VisualTreeHelper.GetParent(source);
            }

            return default(T);
        }
    }
}
