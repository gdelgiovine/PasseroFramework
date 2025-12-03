#if NETFRAMEWORK || NETSTANDARD2_0

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Polyfill for CallerArgumentExpression attribute for older frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter whose expression should be captured.</param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string ParameterName { get; }
    }
}

#endif