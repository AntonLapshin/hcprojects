using System.Collections.ObjectModel;
using System.Linq;

namespace NormManagementMVVM.Model
{
    public static class IQueryableExtender
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IQueryable<TSource> source)
        {
            var oCollection = new ObservableCollection<TSource>();
            foreach (TSource item in source)
            {
                oCollection.Add(item);
            }
            return oCollection;
        }
    }
}