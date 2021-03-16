using System.Windows.Data;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf
{
    public class BindingArray : ArrayExtension
    {
        public BindingArray() : base() => Type = typeof(Binding);
    }
}