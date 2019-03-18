﻿using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class CollectionPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Or || expression.NodeType == ExpressionType.OrElse;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            IPredicateVisitor left = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor right = predicateVisitorFactory.Create(binaryExpression.Right);

            // variants.price.centAmount:range (1 to 30), (40 to 100)
            if (CanCombinePredicateVisitors(left, right))
            {
                return CombinePredicateVisitors(left, right);
            }

            CollectionPredicateVisitor collectionPredicate = new CollectionPredicateVisitor(new List<IPredicateVisitor>() { left, right });
            return collectionPredicate;
        }

        private static bool CanCombinePredicateVisitors(IPredicateVisitor left, IPredicateVisitor right)
        {
            return HasSameParents(left, right);
        }

        private static bool HasSameParents(IPredicateVisitor left, IPredicateVisitor right)
        {
            bool result = false;
            EqualPredicateVisitor equalLeft = left as EqualPredicateVisitor;
            EqualPredicateVisitor equalRight = right as EqualPredicateVisitor;
            IPredicateVisitor leftProperty = equalLeft?.Left;
            IPredicateVisitor rightProperty = equalRight?.Left;

            while (leftProperty is Visitors.AccessorPredicateVisitor accessorLeft && rightProperty is Visitors.AccessorPredicateVisitor accessorRight)
            {
                result = ArePropertiesEqual(accessorLeft, accessorRight);
                leftProperty = accessorLeft.Parent;
                rightProperty = accessorRight.Parent;
            }

            return result;
        }

        private static bool ArePropertiesEqual(Visitors.AccessorPredicateVisitor left, Visitors.AccessorPredicateVisitor right)
        {
            ConstantPredicateVisitor constantLeft = left.Current as ConstantPredicateVisitor;
            ConstantPredicateVisitor constantRight = right.Current as ConstantPredicateVisitor;
            if (constantLeft != null && constantRight != null)
            {
                return constantLeft.Constant == constantRight.Constant;
            }

            return false;
        }

        private static IPredicateVisitor CombinePredicateVisitors(IPredicateVisitor left, IPredicateVisitor right)
        {
            EqualPredicateVisitor equalLeft = left as EqualPredicateVisitor;
            EqualPredicateVisitor equalRight = right as EqualPredicateVisitor;
            Visitors.AccessorPredicateVisitor accessorLeft = equalLeft?.Left as Visitors.AccessorPredicateVisitor;
            Visitors.AccessorPredicateVisitor accessorRight = equalRight?.Left as Visitors.AccessorPredicateVisitor;
            IPredicateVisitor collection;
            if (equalLeft?.Right is RangeCollectionPredicateVisitor rangeLeft &&
                equalRight?.Right is RangeCollectionPredicateVisitor rangeRight)
            {
                List<RangePredicateVisitor> combinedRanges = new List<RangePredicateVisitor>();
                combinedRanges.AddRange(rangeLeft.Ranges);
                combinedRanges.AddRange(rangeRight.Ranges);
                collection = new RangeCollectionPredicateVisitor(combinedRanges);
            }
            else
            {
                collection = new CollectionPredicateVisitor(new List<IPredicateVisitor>() { equalLeft?.Right, equalRight?.Right });
            }

            EqualPredicateVisitor combined = new EqualPredicateVisitor(equalLeft?.Left, collection);
            return combined;
        }
    }
}
