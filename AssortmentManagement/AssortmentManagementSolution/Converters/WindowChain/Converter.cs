using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Converters.WindowChain
{
    public class NullConverterColor : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            if (value == null) return "Black";
            if (int.TryParse(value.ToString(), out result))
            {
                return "White";
            }
            return ((ChainValue)value).State == ValueStates.NotValid || ((ChainValue)value).State == ValueStates.Various
                       ? "Tomato"
                       : "White";
            
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    public class NullConverterValueAction : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            if (value == null) return "Black";
            if (int.TryParse(value.ToString(), out result))
            {
                return "Green";
            }
            if (((ChainValue)value).State == ValueStates.NotValid) return ValueStates.NotValid.Description();
            if (((ChainValue)value).State == ValueStates.Various) return ValueStates.Various.Description();

            var action = (Actions)((ChainValue)value).Value;
            return action.Equals(null) ? ValueStates.NotValid.Description() : action.Description();

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
