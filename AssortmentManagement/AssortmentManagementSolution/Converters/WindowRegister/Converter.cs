using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace AssortmentManagement.Converters.WindowRegister
{
    public class RowValueConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Split('&')[0];
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

    public class RowColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null) return "LightGray";
            //string action = ((string)value).Split('&')[0];
            //int color = System.Convert.ToInt32(((string)value).Split('&')[0]);
            string status = ((string)value).Split('&')[1];
            switch (status)
            {
                case "U":
                    {
                        return "LightGray";
                    }
                case "A":
                    {
                        return "LightSteelBlue";
                    }
                case "R":
                    {
                        return "LightGreen";
                    }
                case "E":
                    {
                        return "Tomato";
                    }
                case "C":
                    {
                        return "LightGoldenrodYellow";
                    }
                case "M":
                    {
                        return "Pink";
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
