﻿namespace Ratcow.DynamicInterface;

// this basically implements AttributedMapper, but it uses 2 instances to create detours

/// <summary>
/// a basic implementation with attributes.
///
/// This is the basic initial version from with a non attributed version will grow
/// </summary>
public class AttributedMapper : V1Mapper
{
    public Type? CreateType<T>(params object[] instances) =>
        CreateTypeImplementation<T>(instances);
    
    protected override Type? CreateTypeImplementation<T>(params object[] instances)
    {
        var interfaceType = typeof(T);
        if (interfaceType.IsInterface)
        {
            var baseName = interfaceType.Name.Substring(1);

            var propertyData = GetPropertyData<T>(instances);
            var methodData = GetMethodData<T>(instances);
            var eventData = GetEventData<T>(instances);

            Thread.GetDomain();
            var assemblyName = new AssemblyName()
            {
                Name = $"{interfaceType.Namespace}.Dynamic"
            };

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var dynamicModule = assemblyBuilder.DefineDynamicModule($"{baseName}Dynamic"); // unique name? $"_{Guid.NewGuid().ToString().Replace('-', '0')}

            var dynamicType = dynamicModule.DefineType(baseName, TypeAttributes.Class | TypeAttributes.Public, null, new Type[] { interfaceType });

            var fieldData = AddFields(dynamicType, instances);

            AddConstructor(dynamicType, fieldData);

            var methods = interfaceType.GetPublicMethods();
            foreach (var method in methods)
            {
                var methodInstance = methodData.FirstOrDefault(p => p.Name == method.Name);
                if (methodInstance.Implementor != null)
                {
                    var field = fieldData.FirstOrDefault(f => f.Name == methodInstance.Implementor.GetType().Name.ToLower());
                    AddMethod(dynamicType, method, methodInstance, field);
                }
            }

            var properties = interfaceType.GetPublicProperties();
            foreach (var property in properties)
            {
                var propertyInstance = propertyData.FirstOrDefault(p => p.Name == property.Name);
                if (propertyInstance.Implementor != null)
                {
                    var field = fieldData.FirstOrDefault(f => f.Name == propertyInstance.Implementor.GetType().Name.ToLower());
                    AddProperty(dynamicType, property, propertyInstance, field);
                }
            }

            var events = interfaceType.GetPublicEvents();
            foreach (var @event in events)
            {
                var eventInstance = eventData.FirstOrDefault(p => p.Name == @event.Name);
                if (eventInstance.Implementor != null)
                {
                    var field = fieldData.FirstOrDefault(f => f.Name == eventInstance.Implementor.GetType().Name.ToLower());
                    AddEvent(dynamicType, @event, eventInstance, field);
                }
            }

            var dType = dynamicType.CreateType();

            return dType;
        }

        return null;
    }

    public T CreateInstance<T>(params object[] instances) => CreateInstanceImplementation<T>(instances);
    
    protected override T CreateInstanceImplementation<T>(params object[] instances) => (T)Activator.CreateInstance(CreateTypeImplementation<T>(instances), instances);

    /// <summary>
    /// Gets the method to type mapping
    /// </summary>
    private (string Name, string ImplementorName, object Implementor)[] GetMethodData<T>(object[] instances)
    {
        var result = new List<(string Name, string ImplementorName, object Implementor)>();
        var interfaceType = typeof(T);

        foreach (var instance in instances)
        {
            var type = instance.GetType();
            var methodInfoArray = GetMethods(type);
            foreach (var methodInfo in methodInfoArray)
            {
                var methodImplementations = (MethodImplementationAttribute[])(methodInfo.GetCustomAttributes(typeof(MethodImplementationAttribute), true));
                foreach (var methodImplementation in methodImplementations)
                {
                    if (methodImplementation.Interface == interfaceType &&
                        methodImplementation.Name is not null)
                    {
                        result.Add((methodImplementation.Name, methodInfo.Name, instance));
                    }
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Gets the property to type mapping
    /// </summary>
    private (string Name, string InstanceName, object Implementor)[] GetPropertyData<T>(object[] instances)
    {
        var result = new List<(string Name, string instanceName, object Implementor)>();
        var interfaceType = typeof(T);

        foreach (var instance in instances)
        {
            var type = instance.GetType();
            var propertyInfoArray = GetProperties(type);
            foreach (var propertyInfo in propertyInfoArray)
            {
                var propertyImplementations = (PropertyImplementationAttribute[])(propertyInfo.GetCustomAttributes(typeof(PropertyImplementationAttribute), true));
                foreach (var propertyImplementation in propertyImplementations)
                {
                    if (propertyImplementation is { Name: {} impName, Interface: {} iType} &&
                        propertyInfo is { Name : {} name} && instance is not null &&
                        (interfaceType == iType || interfaceType.GetInterfaces().Contains(iType)))
                    {
                        result.Add((impName, name, instance));
                    }
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Gets the event to type mapping
    /// </summary>
    private (string Name, string InstanceName, object Implementor)[] GetEventData<T>(object[] instances)
    {
        var result = new List<(string Name, string instanceName, object Implementor)>();
        var interfaceType = typeof(T);

        foreach (var instance in instances)
        {
            var type = instance.GetType();
            var eventInfoArray = GetEvents(type);
            foreach (var eventInfo in eventInfoArray)
            {
                var eventImplementations = (EventImplementationAttribute[])(eventInfo.GetCustomAttributes(typeof(EventImplementationAttribute), true));
                foreach (var eventImplementation in eventImplementations)
                {
                    if (eventImplementation is { Name: {} impName, Interface: {} iType} &&
                        eventInfo is { Name : {} name} && instance is not null &&
                        (interfaceType == iType || interfaceType.GetInterfaces().Contains(iType)))
                    {
                        result.Add((impName, name, instance));
                    }
                }
            }
        }

        return result.ToArray();
    }

}