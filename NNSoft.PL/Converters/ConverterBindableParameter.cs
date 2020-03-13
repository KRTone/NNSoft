using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace NNSoft.PL.Converters
{
    /// <summary>
    /// https://www.codeproject.com/Articles/459958/Bindable-Converter-Converter-Parameter-and-StringF
    /// </summary>
    class ConverterBindableParameter : MarkupExtension
    {
        public Binding Binding { get; set; }

        public IValueConverter Converter { get; set; }

        public Binding ConverterParameterBinding { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(Binding);
            multiBinding.Bindings.Add(ConverterParameterBinding);
            var adapter = new MultiValueConverterAdapter
            {
                Converter = Converter
            };
            multiBinding.Converter = adapter;
            return multiBinding.ProvideValue(serviceProvider);
        }
    }
}
