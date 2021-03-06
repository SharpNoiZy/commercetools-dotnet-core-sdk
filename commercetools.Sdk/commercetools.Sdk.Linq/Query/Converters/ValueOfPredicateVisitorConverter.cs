using System;
using System.Globalization;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    /// <summary>
    /// For comparison of non local values like (c.key == category.key.valueOf())
    /// </summary>
    public class ValueOfPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "valueOf")
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            var compiledValue = Expression.Lambda(expression, null).Compile().DynamicInvoke(null).ToString();
            if ((expression as MethodCallExpression)?.Arguments[0].Type == typeof(string))
            {
                compiledValue = compiledValue.WrapInQuotes();
            }

            return new ConstantPredicateVisitor(compiledValue);
        }
    }
}