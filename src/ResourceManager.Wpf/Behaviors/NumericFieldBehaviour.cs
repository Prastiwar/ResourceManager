using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ResourceManager.Wpf.Behaviors
{
    public class NumericFieldBehaviour : Behavior<TextBox>
    {
        public static readonly DependencyProperty EmptyValueProperty = DependencyProperty.Register("EmptyValue", typeof(string), typeof(NumericFieldBehaviour), null);

        public bool IsPrecise { get; set; }

        public string EmptyValue {
            get => (string)GetValue(EmptyValueProperty);
            set => SetValue(EmptyValueProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(AssociatedObject, OnPasting);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            DataObject.RemovePastingHandler(AssociatedObject, OnPasting);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text;
            if (AssociatedObject.Text.Length < AssociatedObject.CaretIndex)
            {
                text = AssociatedObject.Text;
            }
            else
            {
                //  Remaining text after removing selected text.
                text = TreatSelectedText(out string remainingTextAfterRemoveSelection)
                    ? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
                    : AssociatedObject.Text.Insert(AssociatedObject.CaretIndex, e.Text);
            }
            if (IsPrecise && text.EndsWith('.'))
            {
                text += "0";
            }
            e.Handled = !ValidateText(text);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(EmptyValue))
            {
                return;
            }
            string text = null;

            if (e.Key == Key.Back)
            {
                if (!TreatSelectedText(out text))
                {
                    if (AssociatedObject.SelectionStart > 0)
                    {
                        text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
                    }
                }
            }
            else if (e.Key == Key.Delete)
            {
                // If text was selected, delete it
                if (!TreatSelectedText(out text) && AssociatedObject.Text.Length > AssociatedObject.SelectionStart)
                {
                    // Otherwise delete next symbol
                    text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
                }
            }

            if (text == string.Empty)
            {
                AssociatedObject.Text = EmptyValue;
                if (e.Key == Key.Back)
                {
                    AssociatedObject.SelectionStart++;
                }
                e.Handled = true;
            }
        }

        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));
                if (!ValidateText(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool ValidateText(string text)
        {
            bool isFloat = float.TryParse(text, out _);
            bool canBeFloat = float.TryParse(text.Replace(".", ""), out _) && HasNoMoreThanOneDot(text);
            return isFloat || (IsPrecise && canBeFloat);
        }

        private bool HasNoMoreThanOneDot(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (c == '.')
                {
                    count++;
                }
                else if (c == ',')
                {
                    return false;
                }
                if (count > 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool TreatSelectedText(out string text)
        {
            text = null;
            if (AssociatedObject.SelectionLength <= 0)
            {
                return false;
            }

            int length = AssociatedObject.Text.Length;
            if (AssociatedObject.SelectionStart >= length)
            {
                return true;
            }

            if (AssociatedObject.SelectionStart + AssociatedObject.SelectionLength >= length)
            {
                AssociatedObject.SelectionLength = length - AssociatedObject.SelectionStart;
            }

            text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
            return true;
        }
    }
}
