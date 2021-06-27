using MaterialDesignThemes.Wpf;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ResourceManager.Wpf.Controls
{
    public class MaterialColorPicker : UserControl
    {
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(MaterialColorPicker), new PropertyMetadata(Colors.Black, OnColorChanged));
        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as MaterialColorPicker).Picker_ColorChanged((Color)e.NewValue);
        public Color Color {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        private ColorPicker picker;
        private TextBox hexTextBox;
        private Button colorButton;

        private bool isTextBoxInput;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            picker = Template.FindName("Picker", this) as ColorPicker;
            hexTextBox = Template.FindName("HexTextBox", this) as TextBox;
            colorButton = Template.FindName("ColorButton", this) as Button;

            picker.SetBinding(ColorPicker.ColorProperty, new Binding("Color") { Source = this, Mode = BindingMode.TwoWay });
            hexTextBox.TextChanged += HexTextBox_TextChanged;
            hexTextBox.Text = GetColor();
            colorButton.Click += Button_Click;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ColorProperty)
            {
                Picker_ColorChanged(Color);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            => picker.Visibility = picker.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        private void Picker_ColorChanged(Color newColor)
        {
            if (picker == null)
            {
                return;
            }
            if (newColor.A == 0)
            {
                picker.Color = Color.Add(newColor, Colors.Black);
            }
            picker.Color = newColor;
            if (!isTextBoxInput)
            {
                hexTextBox.Text = GetColor();
            }
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextBoxInput = true;
            try
            {
                Color c = (Color)ColorConverter.ConvertFromString(hexTextBox.Text);
                picker.Color = c;
            }
            catch (Exception)
            {
            }
            isTextBoxInput = false;
        }

        private string GetColor()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("#{0:X2}", picker.Color.R);
            sb.AppendFormat("{0:X2}", picker.Color.G);
            sb.AppendFormat("{0:X2}", picker.Color.B);
            return sb.ToString();
        }
    }
}
