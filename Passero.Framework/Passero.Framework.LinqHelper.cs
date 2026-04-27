using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public class LinqHelper   
    {
        public static  Expression<Func<object, bool>> BuildDynamicLinqQuery(string propertyName, object value, Type ModelType)
        {

            if (ModelType == null)
            {
                throw new ArgumentException("ModelClass must be set before building a LINQ query");
            }

            // Create the parameter of type Object
            var parameter = Expression.Parameter(typeof(object), "x");

            // Convert the Object parameter to the model type
            var convertedParameter = Expression.Convert(parameter, ModelType);

            // Create the property access on the converted parameter
            var property = Expression.Property(convertedParameter, propertyName);

            // Create the constant value with the correct type of the property
            var propertyType = ModelType.GetProperty(propertyName).PropertyType;
            var constant = Expression.Constant(Convert.ChangeType(value, propertyType));

            // Create the equality expression
            var equalExpression = Expression.Equal(property, constant);

            // Create the complete expression tree using the original Object parameter
            return Expression.Lambda<Func<object, bool>>(equalExpression, parameter);
        }


    }
}
