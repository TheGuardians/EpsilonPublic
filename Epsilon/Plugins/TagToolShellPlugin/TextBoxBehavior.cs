using System.Windows;
using System.Windows.Controls;

namespace TagToolShellPlugin
{
    class TextBoxBehavior
    {
        public static int GetCaretIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(CaretIndexProperty);
        }

        public static void SetCaretIndex(DependencyObject obj, int value)
        {
            obj.SetValue(CaretIndexProperty, value);
        }

        public static readonly DependencyProperty CaretIndexProperty =
            DependencyProperty.RegisterAttached("CaretIndex", typeof(int), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(-1, OnCaretIndexChanged));

        private static void OnCaretIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = (TextBox)d;
            if((int)e.OldValue == -1)
                AttachBehavior(textbox);
            textbox.CaretIndex = (int)e.NewValue;
        }

        private static void AttachBehavior(TextBox textbox)
        {
            TextChangedEventHandler handler = (textChangedSender, textChangedEvent) =>
            {
                SetCaretIndex(textbox, textbox.CaretIndex);
            };

            textbox.TextChanged += handler;
        }
    }
}
