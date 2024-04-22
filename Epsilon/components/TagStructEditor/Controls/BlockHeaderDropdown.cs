using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TagStructEditor.Controls
{
    [TemplatePart(Name = ComboBoxPartName, Type = typeof(ComboBox))]
    public class BlockHeaderDropdown : Control
    {
        const string ComboBoxPartName = "PART_Combo";

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(BlockHeaderDropdown), new PropertyMetadata(OnCountChanged));
        public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register("CurrentIndex", typeof(int), typeof(BlockHeaderDropdown), new PropertyMetadata(OnCurrentIndexChanged));

        private ComboBox _combo;
        private bool _dropdownIsOpen;


        static BlockHeaderDropdown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockHeaderDropdown), new FrameworkPropertyMetadata(typeof(BlockHeaderDropdown)));
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }


        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BlockHeaderDropdown).Update();
        }

        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BlockHeaderDropdown).Update();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _combo = GetTemplateChild(ComboBoxPartName) as ComboBox;
            _dropdownIsOpen = false;
            _combo.DropDownOpened -= _combo_DropDownOpened;
            _combo.DropDownClosed -= _combo_DropDownClosed;
            _combo.DropDownOpened += _combo_DropDownOpened;
            _combo.DropDownClosed += _combo_DropDownClosed;
            Update();
        }


        private void _combo_DropDownClosed(object sender, EventArgs e)
        {
            _dropdownIsOpen = false;
            Update();
        }

        private void _combo_DropDownOpened(object sender, EventArgs e)
        {
            _dropdownIsOpen = true;
            Update();
        }

        void Update()
        {
            if (_combo == null)
                return;

            _combo.ItemsSource = GenerateHeaders().ToArray();
        }

        IEnumerable<int> GenerateHeaders()
        {
            if (CurrentIndex < 0 || Count <= 0)
                return Enumerable.Empty<int>();

            if (_dropdownIsOpen)
                return Enumerable.Range(0, Count);
            else
                return new int[] { CurrentIndex };
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!_combo.IsFocused)
                return;

            if (e.Key == Key.Up)
            {
                if (CurrentIndex > 0)
                    CurrentIndex--;

                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (CurrentIndex < Count - 1)
                    CurrentIndex++;

                e.Handled = true;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (!_combo.IsDropDownOpen && !_combo.IsMouseOver)
                return;

            // take focus away from any inputs so that the value is saved
            if(!IsFocused)
                Focus();

            if (e.Delta > 0)
            {
                if (CurrentIndex > 0)
                    CurrentIndex--;

                e.Handled = true;
            }
            if (e.Delta < 0)
            {
                if (CurrentIndex < Count - 1)
                    CurrentIndex++;

                e.Handled = true;
            }
        }
    }
}
