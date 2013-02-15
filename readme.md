AutofacExtensions
=================

AutofacExtensions adds some extension methods and helper modules to make Autofac more pleasant to use.

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

If you have a named parameter that you want to add to every registration, you can use GlobalParameterModule:

```csharp
GlobalParameterModule module = new GlobalParameterModule();
module.AddParameter("parameterOne", /* constant value here */);
module.AddParameter("parameterTwo", c => /* resolution logic here */);

builder.RegisterModule(module);
```

If you have a named property that you want to add to every registration, you can use GlobalPropertyModule:

```csharp
GlobalPropertyModule module = new GlobalPropertyModule();
module.AddProperty("PropertyOne", /* constant value here */);
module.AddProperty("PropertyTwo", c => /* resolution logic here */);

builder.RegisterModule(module);
```

Copyright
---------
Copyright Matthew King 2012.

License
-------
AutofacExtensions is licensed under the [Boost Software License](http://www.boost.org/users/license.html). Refer to license.txt for more information.