using RPGDataEditor.Wpf.Converters;
using System.Linq;
using System.Windows.Data;

namespace RPGDataEditor.Wpf
{
    /// <summary> Returns itself (MultiBinding) as value </summary>
    public class MultiBindingValue : MultiBinding
    {
        public MultiBindingValue() => Converter = StaticValueConverter.CreateMulti(values => this, multiBinding => multiBinding.Bindings.Cast<object>().ToArray());

        public MultiBinding ToMultiBinding(IMultiValueConverter converter)
        {
            MultiBinding multiBinding = (MultiBinding)this.CloneBinding();
            multiBinding.Converter = converter;
            return multiBinding;
        }
    }
}
