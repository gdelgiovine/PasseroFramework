using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Passero.Framework
{
    /// <summary>
    ///   <br />
    /// </summary>
    public static class AttributeHelper
    {

        /// <summary>Gets the property attribute value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the out.</typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.MissingMemberException"></exception>
        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(Expression<Func<T, TOut>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

            if (attr == null)
            {
                throw new MissingMemberException(typeof(T).Name + "." + propertyInfo.Name, typeof(TAttribute).Name);
            }

            return valueSelector(attr);
        }

    }
}
