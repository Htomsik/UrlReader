using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Core.Models;

namespace UrlReader.Infrastructure.Converters;

public class IsMaxValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is null || values.Length == 0)
            return false;

        ICollection<ServiceUrl>? serviceUrls = null;

        int? tagsCount = null;

        if (values[0] is ObservableCollection<ServiceUrl> && values[1] is int)
        {
            serviceUrls = (ObservableCollection<ServiceUrl>)values[0];
            tagsCount = (int)values[1];
        }

        if (serviceUrls is null || tagsCount is null || tagsCount == 0)
            return false;

        return serviceUrls.Max(x => x.TagsCount) == tagsCount;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException(nameof(IsMaxValueConverter.ConvertBack));

}
    