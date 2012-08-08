using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NormManagementMVVM.Model
{
    public static class IEnumerableExtender
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new Exception("source is null");
            var oCollection = new ObservableCollection<TSource>();
            foreach (TSource item in source)
            {
                oCollection.Add(item);
            }
            return oCollection;
        }
    }
}