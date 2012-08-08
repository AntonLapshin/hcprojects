using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedComponents.Linq.Extension
{
    /// <summary>
    /// http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </summary>
    public static class ExpressionExtension
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
                                               Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            Dictionary<ParameterExpression, ParameterExpression> map =
                first.Parameters.Select((f, i) => new {f, s = second.Parameters[i]}).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            System.Linq.Expressions.Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return System.Linq.Expressions.Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
                                                       Expression<Func<T, bool>> second)
        {
            return first.Compose(second, System.Linq.Expressions.Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
                                                      Expression<Func<T, bool>> second)
        {
            return first.Compose(second, System.Linq.Expressions.Expression.Or);
        }
    }
}