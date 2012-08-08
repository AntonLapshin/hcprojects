using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace AssortmentManagement.Converters.PivotGridControlModified
{
    public class ColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "LightGray";
            string action = ((string)value).Split('&')[0];
            //int color = System.Convert.ToInt32(((string)value).Split('&')[0]);
            switch (action)
            {
                case "00":
                    {
                        return "White";
                    }
                case "10":
                    {
                        return "LightGreen";
                    }
                case "01":
                    {
                        return "Red";
                    }
                case "11":
                    {
                        return "Yellow";
                    }
            }
            return "LightGray";
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
    public class ValueConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return -9;
            return ((string)value).Split('&')[2];
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
    public class VisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Hidden";
            return System.Convert.ToInt32(((string)value).Split('&')[2]) == 0 ? "Hidden" : "Visible";
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
    public class ColorBackgroundModifyConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "LightGray";
            int modify = System.Convert.ToInt32(((string)value).Split('&')[1]);
            switch (modify)
            {
                case 0:
                    {
                        return "White";
                    }
                case 1:
                    {
                        return "Black";
                    }
            }
            return "LightGray";
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
    public class ColorForegroundModifyConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "LightGray";
            int modify = System.Convert.ToInt32(((string)value).Split('&')[1]);
            switch (modify)
            {
                case 1:
                    {
                        return "White";
                    }
                case 0:
                    {
                        return "Black";
                    }
            }
            return "LightGray";
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
