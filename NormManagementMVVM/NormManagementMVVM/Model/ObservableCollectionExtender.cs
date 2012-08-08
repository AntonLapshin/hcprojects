using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NormManagementMVVM.Model
{
    public static class ObservableCollectionExtender
    {
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source,
                                               Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (TSource sortedItem in sortedList)
                source.Add(sortedItem);
        }

        public static void SortDesc<TSource, TKey>(this ObservableCollection<TSource> source,
                                                   Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderByDescending(keySelector).ToList();
            source.Clear();
            foreach (TSource sortedItem in sortedList)
                source.Add(sortedItem);
        }
    }
}