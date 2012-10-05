namespace AutofacExtensions.Tests
{
    using System;
    using Autofac;
    using Autofac.Builder;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class AutofacExtensionsTests
    {
        public class ExampleType
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }

            public ExampleType() { }

            public ExampleType(string value1, string value2)
            {
                this.Value1 = value1;
                this.Value2 = value2;
            }
        }

        [Test]
        public void WithParameter_RegistrationIsNull_ThrowsArgumentNullException()
        {
            IRegistrationBuilder<ExampleType, ReflectionActivatorData, object> regBuilder = null;
            string parameterName = "value1";
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "registration should not be null.";
            Assert.That(() => AutofacExtensions.WithParameter(regBuilder, parameterName, accessor),
                Throws.TypeOf<ArgumentNullException>()
                      .And.Message.Contains(message));
        }

        [Test]
        public void WithParameter_ParameterNameIsNull_ThrowsArgumentNullException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string parameterName = null;
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "parameterName should not be null.";
            Assert.That(() => AutofacExtensions.WithParameter(regBuilder, parameterName, accessor),
               Throws.TypeOf<ArgumentNullException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithParameter_ParameterNameIsEmpty_ThrowsArgumentException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string parameterName = String.Empty;
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "parameterName should not be an empty string.";
            Assert.That(() => AutofacExtensions.WithParameter(regBuilder, parameterName, accessor),
               Throws.TypeOf<ArgumentException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithParameter_AccessorIsNull_ThrowsArgumentNullException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string parameterName = "value1";
            Func<IComponentContext, object> accessor = null;

            const string message = "valueAccessor should not be null.";
            Assert.That(() => AutofacExtensions.WithParameter(regBuilder, parameterName, accessor),
               Throws.TypeOf<ArgumentNullException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithParameter_Integration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<string>("example_value_one")
                .Keyed<string>("key_one")
                .SingleInstance();

            builder.RegisterInstance<string>("example_value_two")
                .Keyed<string>("key_two")
                .SingleInstance();

            builder.RegisterType<ExampleType>()
                .WithParameter("value1", c => c.ResolveKeyed<string>("key_one"))
                .WithParameter("value2", c => c.ResolveKeyed<string>("key_two"))
                .SingleInstance();

            using (var container = builder.Build())
            {
                var example = container.ResolveOptional<ExampleType>();
                Assert.That(example, Is.Not.Null);
                Assert.That(example.Value1, Is.EqualTo("example_value_one"));
                Assert.That(example.Value2, Is.EqualTo("example_value_two"));
            }
        }

        [Test]
        public void WithProperty_RegistrationIsNull_ThrowsArgumentNullException()
        {
            IRegistrationBuilder<ExampleType, ReflectionActivatorData, object> regBuilder = null;
            string propertyName = "Value1";
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "registration should not be null.";
            Assert.That(() => AutofacExtensions.WithProperty(regBuilder, propertyName, accessor),
                Throws.TypeOf<ArgumentNullException>()
                      .And.Message.Contains(message));
        }

        [Test]
        public void WithProperty_PropertyNameIsNull_ThrowsArgumentNullException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string propertyName = null;
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "propertyName should not be null.";
            Assert.That(() => AutofacExtensions.WithProperty(regBuilder, propertyName, accessor),
               Throws.TypeOf<ArgumentNullException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithProperty_PropertyNameIsEmpty_ThrowsArgumentException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string propertyName = String.Empty;
            Func<IComponentContext, object> accessor = c => new object();

            const string message = "propertyName should not be an empty string.";
            Assert.That(() => AutofacExtensions.WithProperty(regBuilder, propertyName, accessor),
               Throws.TypeOf<ArgumentException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithProperty_AccessorIsNull_ThrowsArgumentNullException()
        {
            var regBuilder = Substitute.For<IRegistrationBuilder<ExampleType, ReflectionActivatorData, object>>();
            string propertyName = "Value1";
            Func<IComponentContext, object> accessor = null;

            const string message = "valueAccessor should not be null.";
            Assert.That(() => AutofacExtensions.WithProperty(regBuilder, propertyName, accessor),
               Throws.TypeOf<ArgumentNullException>()
                     .And.Message.Contains(message));
        }

        [Test]
        public void WithProperty_Integration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<string>("example_value_one")
                .Keyed<string>("key_one")
                .SingleInstance();

            builder.RegisterInstance<string>("example_value_two")
                .Keyed<string>("key_two")
                .SingleInstance();

            builder.RegisterType<ExampleType>()
                .WithProperty("Value1", c => c.ResolveKeyed<string>("key_one"))
                .WithProperty("Value2", c => c.ResolveKeyed<string>("key_two"))
                .SingleInstance();

            using (var container = builder.Build())
            {
                var example = container.ResolveOptional<ExampleType>();
                Assert.That(example, Is.Not.Null);
                Assert.That(example.Value1, Is.EqualTo("example_value_one"));
                Assert.That(example.Value2, Is.EqualTo("example_value_two"));
            }
        }
    }
}
