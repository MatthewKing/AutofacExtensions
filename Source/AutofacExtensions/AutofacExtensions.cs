namespace Autofac
{
    using System;
    using System.Reflection;
    using Autofac.Builder;
    using Autofac.Core;

    /// <summary>
    /// Extension methods for Autofac..
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// Configure a resolved value for a constructor parameter.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TReflectionActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="registration">
        /// Registration to set parameter on.
        /// </param>
        /// <param name="parameterName">
        /// Name of a constructor parameter on the target type.
        /// </param>
        /// <param name="valueAccessor">
        /// A function that supplies the parameter value given the context.
        /// </param>
        /// <returns>
        /// A registration builder allowing further configuration of the component.
        /// </returns>
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle>
            WithParameter<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
                string parameterName,
                Func<IComponentContext, object> valueAccessor)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            if (registration == null)
                throw new ArgumentNullException(
                    "registration",
                    "registration should not be null.");

            if (parameterName == null)
                throw new ArgumentNullException(
                    "parameterName",
                    "parameterName should not be null.");

            if (parameterName.Length == 0)
                throw new ArgumentException(
                    "parameterName should not be an empty string.",
                    "parameterName");

            if (valueAccessor == null)
                throw new ArgumentNullException(
                    "valueAccessor",
                    "valueAccessor should not be null.");

            ResolvedParameter parameter = new ResolvedParameter(
                (p, c) => String.Equals(p.Name, parameterName),
                (p, c) => valueAccessor(c));

            return registration.WithParameter(parameter);
        }

        /// <summary>
        /// Configure a resolved value for a property.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TReflectionActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="registration">
        /// Registration to set property on.
        /// </param>
        /// <param name="propertyName">
        /// Name of a property on the target type.
        /// </param>
        /// <param name="valueAccessor">
        /// A function that supplies the property value given the context.
        /// </param>
        /// <returns>
        /// A registration builder allowing further configuration of the component.
        /// </returns>
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle>
            WithProperty<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
                string propertyName,
                Func<IComponentContext, object> valueAccessor)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            if (registration == null)
                throw new ArgumentNullException(
                    "registration",
                    "registration should not be null.");

            if (propertyName == null)
                throw new ArgumentNullException(
                    "propertyName",
                    "propertyName should not be null.");

            if (propertyName.Length == 0)
                throw new ArgumentException(
                    "propertyName should not be an empty string.",
                    "propertyName");

            if (valueAccessor == null)
                throw new ArgumentNullException(
                    "valueAccessor",
                    "valueAccessor should not be null.");

            ResolvedParameter parameter = new ResolvedParameter(
                (p, c) =>
                {
                    MethodInfo m = p.Member as MethodInfo;
                    return (m != null && m.IsSpecialName && m.Name.StartsWith("set_"))
                        ? String.Equals(propertyName, m.Name.Substring(4))
                        : false;
                },
                (p, c) => valueAccessor(c));

            return registration.WithProperty(parameter);
        }
    }
}
