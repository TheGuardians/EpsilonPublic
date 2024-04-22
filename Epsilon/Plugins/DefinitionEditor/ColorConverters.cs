using ColorPicker.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using TagStructEditor.Fields;
using TagTool.Common;

namespace DefinitionEditor
{
    public abstract class MultiBinderBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetElement = targetProvider.TargetObject as FrameworkElement;
            var targetProperty = targetProvider.TargetProperty as DependencyProperty;

            if (targetElement != null && targetProperty != null)
            {
                // make sure that if the binding context changes then the binding gets updated.
                targetElement.DataContextChanged += (sender, args) => ApplyBinding(targetElement, targetProperty, args.NewValue);

                var binding = ApplyBinding(targetElement, targetProperty, targetElement.DataContext);
                return binding.ProvideValue(serviceProvider);
            }

            return Binding.DoNothing;
        }

        private BindingBase ApplyBinding(DependencyObject target, DependencyProperty property, object source)
        {
            BindingOperations.ClearBinding(target, property);
            return Bind(source);
        }

        public abstract MultiBinding Bind(object source);
    }

    public class RealRgbColorBinder : MultiBinderBase, IMultiValueConverter
    {
        public override MultiBinding Bind(object source)
        {
            var binding = new MultiBinding() { Mode = BindingMode.TwoWay, Converter = this };
            binding.Bindings.Add(new Binding(nameof(RealRgbColorField.Red)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealRgbColorField.Green)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealRgbColorField.Blue)) { Mode = BindingMode.TwoWay, Source = source, });
            return binding;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var cs = new ColorState();
            cs.SetARGB(1.0, (float)values[0], (float)values[1], (float)values[2]);
            return cs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var cs = (ColorState)value;
            return new object[] { (float)cs.RGB_R, (float)cs.RGB_G, (float)cs.RGB_B };
        }
    }

    public class RealArgbColorBinder : MultiBinderBase, IMultiValueConverter
    {
        public override MultiBinding Bind(object source)
        {
            var binding = new MultiBinding() { Mode = BindingMode.TwoWay, Converter = this };
            binding.Bindings.Add(new Binding(nameof(RealArgbColorField.Alpha)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealArgbColorField.Red)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealArgbColorField.Green)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealArgbColorField.Blue)) { Mode = BindingMode.TwoWay, Source = source, });
            return binding;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var cs = new ColorState();
            cs.SetARGB((float)values[0], (float)values[1], (float)values[2], (float)values[3]);
            return cs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var cs = (ColorState)value;
            return new object[] { (float)cs.A, (float)cs.RGB_R, (float)cs.RGB_G, (float)cs.RGB_B };
        }
    }

    public class RealRgbaColorBinder : MultiBinderBase, IMultiValueConverter
    {
        public override MultiBinding Bind(object source)
        {
            var binding = new MultiBinding() { Mode = BindingMode.TwoWay, Converter = this };
            binding.Bindings.Add(new Binding(nameof(RealRgbaColorField.Alpha)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealRgbaColorField.Red)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealRgbaColorField.Green)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(RealRgbaColorField.Blue)) { Mode = BindingMode.TwoWay, Source = source, });
            return binding;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var cs = new ColorState();
            cs.SetARGB((float)values[0], (float)values[1], (float)values[2], (float)values[3]);
            return cs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var cs = (ColorState)value;
            return new object[] { (float)cs.A, (float)cs.RGB_R, (float)cs.RGB_G, (float)cs.RGB_B };
        }
    }

    public class ArgbColorBinder : MultiBinderBase, IMultiValueConverter
    {
        public override MultiBinding Bind(object source)
        {
            var binding = new MultiBinding() { Mode = BindingMode.TwoWay, Converter = this };
            binding.Bindings.Add(new Binding(nameof(ArgbColorField.Alpha)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(ArgbColorField.Red)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(ArgbColorField.Green)) { Mode = BindingMode.TwoWay, Source = source, });
            binding.Bindings.Add(new Binding(nameof(ArgbColorField.Blue)) { Mode = BindingMode.TwoWay, Source = source, });
            return binding;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var cs = new ColorState();
            var floatValues = ByteToNormalizedFloat(values);
            cs.SetARGB(floatValues[0], floatValues[1], floatValues[2], floatValues[3]);
            return cs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var cs = (ColorState)value;
            var byteValues = ColorStateToBytes(cs);
            return new object[] { byteValues[0], byteValues[1], byteValues[2], byteValues[3] };
        }

        public float[] ByteToNormalizedFloat(object[] byteValues)
        {
            float[] newVals = new float[byteValues.Length];
            for(int i = 0; i < byteValues.Length; i++)
            {
                newVals[i] = System.Convert.ToSingle((byte)byteValues[i]) / 255;
            }

            return newVals;
        }

        public byte[] ColorStateToBytes(ColorState cs)
        {
            double[] oldVals = new double[] { cs.A, cs.RGB_R, cs.RGB_G, cs.RGB_B };
            byte[] newVals = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                newVals[i] = System.Convert.ToByte(oldVals[i] * 255);
            }

            return newVals;
        }
    }

    public class FunctionColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var argb = (ArgbColor)value;
            var bytes = new byte[] { argb.Alpha, argb.Red, argb.Green, argb.Blue };
            float[] floats = ByteToNormalizedFloat(bytes);

            var cs = new ColorState();
            cs.SetARGB(floats[0], floats[1], floats[2], floats[3]);
            return cs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = ColorStateToBytes((ColorState)value);
            var argb = new ArgbColor() { Alpha = bytes[0], Red = bytes[1], Green = bytes[2], Blue = bytes[3] };
            return argb;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public float[] ByteToNormalizedFloat(byte[] byteValues)
        {
            float[] newVals = new float[byteValues.Length];
            for (int i = 0; i < byteValues.Length; i++)
            {
                newVals[i] = System.Convert.ToSingle((byte)byteValues[i]) / 255;
            }

            return newVals;
        }

        public byte[] ColorStateToBytes(ColorState cs)
        {
            double[] oldVals = new double[] { cs.A, cs.RGB_R, cs.RGB_G, cs.RGB_B };
            byte[] newVals = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                newVals[i] = System.Convert.ToByte(oldVals[i] * 255);
            }

            return newVals;
        }
    }

    public class HexColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hex = value == null ? "#AA6611" : (string)value;

            Color color = (Color)ColorConverter.ConvertFromString(hex);

            var cs = new ColorState();
            cs.SetARGB(color.A/255f, color.R/255f, color.G/255f, color.B/255f);
            return cs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = ColorStateToBytes((ColorState)value);
            string hex = "#";

            foreach (var b in bytes)
                hex += b.ToString("X2");

            return hex;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public float[] ByteToNormalizedFloat(byte[] byteValues)
        {
            float[] newVals = new float[byteValues.Length];

            for (int i = 0; i < byteValues.Length; i++)
                newVals[i] = System.Convert.ToSingle((byte)byteValues[i]) / 255;

            return newVals;
        }

        public byte[] ColorStateToBytes(ColorState cs)
        {
            double[] oldVals = new double[] { cs.RGB_R, cs.RGB_G, cs.RGB_B };
            byte[] newVals = new byte[3];

            for (int i = 0; i < 3; i++)
                newVals[i] = System.Convert.ToByte(oldVals[i] * 255);

            return newVals;
        }
    }
}
