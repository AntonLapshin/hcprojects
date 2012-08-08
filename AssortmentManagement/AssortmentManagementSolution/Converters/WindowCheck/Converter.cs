using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Converters.WindowCheck
{
    public class ConverterSelected : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selected = System.Convert.ToBoolean(value);
            return selected ? "LightGray" : "White";
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
    public class ConverterColor : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            switch (status)
            {
                case CheckStatuses.Success:
                    {
                        return "DarkGray";
                    }
                case CheckStatuses.Error:
                    {
                        return "DarkGray";
                    }
                case CheckStatuses.Warning:
                    {
                        return "DarkGray";
                    }
                case CheckStatuses.Executing:
                    {
                        return "DarkGray";
                    }
            }
            return "White";
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
    public class ConverterStatusExecuting : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            if (status == CheckStatuses.Executing) return "Visible";
            return "Hidden";
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
    public class ConverterStatusSuccess : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            if (status == CheckStatuses.Success) return "Visible";
            return "Hidden";
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
    public class ConverterStatusError : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            if (status == CheckStatuses.Error) return "Visible";
            return "Hidden";
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
    public class ConverterStatusWarning : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            if (status == CheckStatuses.Warning) return "Visible";
            return "Hidden";
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
    public class ConverterBorder : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (CheckStatuses)System.Convert.ToInt32(value);
            if (status == CheckStatuses.Executing) return "1";
            return "0";
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
