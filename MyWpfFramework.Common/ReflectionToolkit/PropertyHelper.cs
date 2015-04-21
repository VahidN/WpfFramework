using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyWpfFramework.Common.ReflectionToolkit
{
    //from: http://www.martinwilley.com/net/code/reflection/staticreflection.html

    /// <summary>
    /// Get the string name and type of a property or field. Eg <c>string name = Property.Name&lt;string&gt;(x =&gt; x.Length);</c>
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Gets the type for the specified entity property or field. Eg <c>string name = Property.Name&lt;string&gt;(x =&gt; x.Length);</c>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>The name of the property as a string</returns>
        public static string Name<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var memberExpression = getMemberExpression(expression);

            var propertyInfo = memberExpression.Member;
            return propertyInfo.Name;
        }

        /// <summary>
        /// Gets the type for the specified entity property or field. Eg Type&lt;string&gt;(x =&gt; x.Length) == typeof(int)
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>A type.</returns>
        public static Type Type<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var memberExpression = getMemberExpression(expression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.PropertyType;

            //not a property, maybe a public field
            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.FieldType;

            //unknown
            return typeof(object);
        }

        private static MemberExpression getMemberExpression<TEntity, T>(Expression<Func<TEntity, T>> expression)
        {
            //originally from Fluent NHibernate
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            //runtime exception if not a member
            if (memberExpression == null)
                throw new ArgumentException("Not a property or field", "expression");

            return memberExpression;
        }
    }
}