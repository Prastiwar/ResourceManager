using MaterialDesignThemes.Wpf;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace RPGDataEditor.Wpf.Views
{
    public partial class MaterialColorPicker : UserControl
    {
        public MaterialColorPicker()
        {
            InitializeComponent();
            Picker.SetBinding(ColorPicker.ColorProperty, new Binding("Color") { Source = this, Mode = BindingMode.TwoWay });
            HexTextBox.Text = GetColor();
        }

        private bool isTextBoxInput;

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(MaterialColorPicker), new PropertyMetadata(Colors.Black));
        public Color Color {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
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
            => Picker.Visibility = Picker.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        private void Picker_ColorChanged(Color newColor)
        {
            if (newColor.A == 0)
            {
                Picker.Color = Color.Add(newColor, Colors.Black);
            }
            if (!isTextBoxInput)
            {
                HexTextBox.Text = GetColor();
            }
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextBoxInput = true;
            try
            {
                Color c = (Color)ColorConverter.ConvertFromString(HexTextBox.Text);
                Picker.Color = c;
            }
            catch (Exception)
            {
            }
            isTextBoxInput = false;
        }

        private string GetColor()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("#{0:X2}", Picker.Color.R);
            sb.AppendFormat("{0:X2}", Picker.Color.G);
            sb.AppendFormat("{0:X2}", Picker.Color.B);
            return sb.ToString();
        }
    }
}
