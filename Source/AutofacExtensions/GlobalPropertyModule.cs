namespace Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Autofac;
    using Autofac.Core;

    /// <summary>
    /// A Module that adds the specified properties to every component registration.
    /// </summary>
    public sealed class GlobalPropertyModule : Autofac.Module
    {
        /// <summary>
        /// BindingFlags to use when getting a property from a service type.
        /// </summary>
        private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// A dictionary mapping property names to constant values.
        /// </summary>
        private readonly IDictionary<string, object> propertiesConstant;

        /// <summary>
        /// A dictionary mapping property names to resolved values.
        /// </summary>
        private readonly IDictionary<string, Func<IComponentContext, object>> propertiesResolved;

        /// <summary>
        /// Initializes a new instance of the GlobalPropertyModule class.
        /// </summary>
        public GlobalPropertyModule()
        {
            this.propertiesConstant = new Dictionary<string, object>();
            this.propertiesResolved = new Dictionary<string, Func<IComponentContext, object>>();
        }

        /// <summary>
        /// Adds a property with a constant value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The property value.</param>
        public GlobalPropertyModule AddProperty(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(
                    "name",
                    "name should not be null.");

            if (name.Length == 0)
                throw new ArgumentException(
                    "name should not be an empty string.",
                    "name");

            this.propertiesConstant[name] = value;

            return this;
        }

        /// <summary>
        /// Adds a property with a resolved value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="valueAccessor">A function that supplies the property value.</param>
        public GlobalPropertyModule AddProperty(string name, Func<IComponentContext, object> valueAccessor)
        {
            if (name == null)
                throw new ArgumentNullException(
                    "name",
                    "name should not be null.");

            if (name.Length == 0)
                throw new ArgumentException(
                    "name should not be an empty string.",
                    "name");

            if (valueAccessor == null)
                throw new ArgumentNullException(
                    "valueAccessor",
                    "valueAccessor should not be null.");

            this.propertiesResolved[name] = valueAccessor;

            return this;
        }

        /// <summary>
        /// Provides module-specific functionality to a component registration.
        /// </summary>
        /// <param name="componentRegistry">The component registry.</param>
        /// <param name="registration">The registration to attach functionality to.</param>
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            Type limitType = registration.Activator.LimitType;
            if (limitType != null)
            {
                foreach (string propertyName in this.propertiesConstant.Keys)
                {
                    PropertyInfo pi = limitType.GetProperty(propertyName, flags);
                    if (pi != null && pi.GetSetMethod(false) != null)
                    {
                        registration.Activated += (s, e) =>
                        {
                            pi.SetValue(
                                e.Instance,
                                this.propertiesConstant[propertyName],
                                null);
                        };
                    }
                }

                foreach (string propertyName in this.propertiesResolved.Keys)
                {
                    PropertyInfo pi = limitType.GetProperty(propertyName, flags);
                    if (pi != null && pi.GetSetMethod(false) != null)
                    {
                        registration.Activated += (s, e) =>
                        {
                            pi.SetValue(
                                e.Instance,
                                this.propertiesResolved[propertyName](e.Context),
                                null);
                        };
                    }
                }
            }
        }
    }
}
