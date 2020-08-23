using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Extensions
{
    /// <summary>
    /// Lamda 表达式 扩展
    /// </summary>
    public static class PredicateExtensions
    {
        /// <summary>
        /// 返回true表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 返回false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// 放回And表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// 返回Or表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// 返回Order By表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "Not Exist");

            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// 动态创建lab
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildPropertyInExpression<T>(string propName, IEnumerable values)
        {
            ParameterExpression x = Expression.Parameter(typeof(T), "x");
            LambdaExpression expr;
            var tempExp = BuildPropertyInExpressionCore(x, propName, values);
            if (tempExp == null)
            {
                expr = Expression.Lambda(Expression.Constant(false), x);
            }
            else
            {
                expr = Expression.Lambda(tempExp, x);
            }
            return (Expression<Func<T, bool>>)expr;
        }

        /// <summary>
        /// 构建属性等于单个值的表达式
        /// </summary>
        /// <param name="x"></param>
        /// <param name="propName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static BinaryExpression BuildPropertyEqualsSingleValueExpression(ParameterExpression x, string propName, object id)
        {
            MemberExpression left = Expression.Property(x, propName);
            ConstantExpression right = Expression.Constant(id);
            return Expression.Equal(left, right);
        }

        /// <summary>
        /// 构建 属性A等于值1 或者 属性A等于值2 或者 属性A等于值3  等等一个或多个 OrElse  的表达式的核心
        /// </summary>
        /// <param name="x"></param>
        /// <param name="propName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static BinaryExpression BuildPropertyInExpressionCore(ParameterExpression x, string propName, IEnumerable values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            IEnumerator enumerator = values.GetEnumerator();
            int i = 0;
            BinaryExpression binaryExp = null;
            while (enumerator.MoveNext())
            {
                object objValue = enumerator.Current;
                if (objValue == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    binaryExp = BuildPropertyEqualsSingleValueExpression(x, propName, objValue);
                    i++;
                    continue;
                }
                binaryExp = Expression.OrElse(binaryExp, BuildPropertyEqualsSingleValueExpression(x, propName, objValue));
                i++;
            }
            return binaryExp;
        }
    


}

    /// <summary>
    /// 统一ParameterExpression
    /// </summary>
    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        public ParameterExpression ParameterExpression { get; private set; }

        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
}
