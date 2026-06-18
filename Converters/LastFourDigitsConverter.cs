using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Kursach.Converters;


public class LastFourDigitsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string phone && phone.Length >= 4)
            return phone[^4..];
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}