﻿namespace Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Autofac.Core;

    /// <summary>
    /// A Module that adds the specified parameters to every component registration.
    /// </summary>
    public sealed class GlobalParameterModule : Autofac.Module
    {
        /// <summary>
        /// A list containing the parameters that will be added to every registration.
        /// </summary>
        private readonly IList<Parameter> parameters;

        /// <summary>
        /// Initializes a new instance of the GlobalParameterModule class.
        /// </summary>
        public GlobalParameterModule()
        {
            this.parameters = new List<Parameter>();
        }

        /// <summary>
        /// Adds a parameter with a constant value.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This GlobalParameterModule instance.</returns>
        public GlobalParameterModule AddParameter(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(
                    "name",
                    "name should not be null.");

            if (name.Length == 0)
                throw new ArgumentException(
                    "name should not be an empty string.",
                    "name");

            NamedParameter parameter = new NamedParameter(name, value);

            this.parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Adds a parameter with a resolved value.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="valueAccessor">A function that supplies the parameter value.</param>
        /// <returns>This GlobalParameterModule instance.</returns>
        public GlobalParameterModule AddParameter(string name, Func<IComponentContext, object> valueAccessor)
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

            const StringComparison sc = StringComparison.Ordinal;

            ResolvedParameter parameter = new ResolvedParameter(
                (p, c) => String.Equals(p.Name, name, sc),
                (p, c) => valueAccessor(c));

            this.parameters.Add(parameter);

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
            registration.Preparing += (s, e) =>
            {
                e.Parameters = e.Parameters.Union(this.parameters);
            };
        }
    }
}
