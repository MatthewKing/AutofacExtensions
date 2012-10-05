AutofacExtensions
=================

AutofacExtensions adds two new extension methods for use with Autofac.

Currently, if you want to wire up a resolved parameter, you have to do something like:

```csharp
builder.RegisterType<ExampleType>()
    .WithParameter(new ResolvedParameter((parameterInfo, componentContext) => String.Equals(parameterInfo.Name, "parameterName"), (parameterInfo, componentContext) => /* resolution logic here */))
    .InstancePerDependency();
```

AutofacExtensions allows you to do this in a much cleaner way:

```csharp
builder.RegisterType<ExampleType>()
    .WithParameter("parameterName", c => /* resolution logic here */)
    .InstancePerDependency();
```

Resolved properties can be registered in the same way:

```csharp
builder.RegisterType<ExampleType>()
    .WithProperty("PropertyName", c => /* resolution logic here */)
    .InstancePerDependency();
```

Copyright
---------
Copyright Matthew King 2012.

License
-------
AutofacExtensions is licensed under the [Boost Software License](http://www.boost.org/users/license.html). Refer to license.txt for more information.