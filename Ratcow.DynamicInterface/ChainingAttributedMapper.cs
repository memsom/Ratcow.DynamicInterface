namespace Ratcow.DynamicInterface;

public class ChainingAttributedMapper : V1Mapper
{
    public Type? CreateType<T>(object detour, object fallback) =>
        CreateTypeImplementation<T>(detour, fallback);
    
    protected override Type? CreateTypeImplementation<T>(params object[] instances)
    {
        var interfaceType = typeof(T);
        if (interfaceType.IsInterface && instances is [{} detour, {}  fallback])
        {
            var baseName = interfaceType.Name[1..];

            var fallbackType = fallback.GetType();
            
            // verify the fallback implements the interface
            if (!interfaceType.IsAssignableFrom(fallbackType))
            {
                throw new ArgumentException(nameof(fallback));
            }
            
            var detourType = detour.GetType();

            var propertyData = GetPropertyData<T>(detourType, detour, fallback);
            var methodData = GetMethodData<T>(detourType, detour, fallback);
            var eventData = GetEventData<T>(detourType, detour, fallback);

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
                    var field = fieldData.FirstOrDefault(f => f.Name == methodInstance.Implementor.GetType().Name.ToLower()) ?? throw new EngineException("Implementor not found");
                    AddMethod(dynamicType, method, methodInstance, field);
                }
            }

            var properties = interfaceType.GetPublicProperties();
            foreach (var property in properties)
            {
                var propertyInstance = propertyData.FirstOrDefault(p => p.Name == property.Name);
                if (propertyInstance.Implementor != null)
                {
                    var field = fieldData.FirstOrDefault(f => f.Name == propertyInstance.Implementor.GetType().Name.ToLower())?? throw new EngineException("Implementor not found");
                    AddProperty(dynamicType, property, propertyInstance, field);
                }
            }

            var events = interfaceType.GetPublicEvents();
            foreach (var @event in events)
            {
                var eventInstance = eventData.FirstOrDefault(p => p.Name == @event.Name);
                if (eventInstance.Implementor != null)
                {
                    var field = fieldData.FirstOrDefault(f => f.Name == eventInstance.Implementor.GetType().Name.ToLower())?? throw new EngineException("Implementor not found");
                    AddEvent(dynamicType, @event, eventInstance, field);
                }
            }

            var dType = dynamicType.CreateType();

            return dType;
        }

        return null;
    }

    public T CreateInstance<T>(object detour, object fallback) => 
        (T)Activator.CreateInstance(CreateType<T>(detour, fallback)?? throw new EngineException("Type could not be created"), detour, fallback);

    protected override T CreateInstanceImplementation<T>(params object[] instances) => throw new EngineException();

    /// <summary>
    /// Gets the method to type mapping
    /// </summary>
    private (string Name, string ImplementorName, object Implementor)[] GetMethodData<T>(Type detourType, object detour, object fallback)
    {
        var result = new List<(string Name, string ImplementorName, object Implementor)>();
        var interfaceType = typeof(T);
        
        var methodInfoArray = GetMethods(interfaceType);
        foreach (var methodInfo in methodInfoArray)
        {
            if (detourType.GetMethod(methodInfo.Name) is { } dp)
            {
                var methodImplementations = (MethodImplementationAttribute[])(dp.GetCustomAttributes(typeof(MethodImplementationAttribute), true));
                foreach (var methodImplementation in methodImplementations)
                {
                    if (methodImplementation is { Name: {} impName, Interface: {} iType} &&
                        dp is { Name : {} name} && detour is not null &&
                        (interfaceType == iType || interfaceType.GetInterfaces().Contains(iType)))
                    {
                        result.Add((impName, name, detour));
                    }
                }
            }
            else
            {
                result.Add((methodInfo.Name, methodInfo.Name, fallback));
            }
        }
        
        return result.ToArray();
    }

    /// <summary>
    /// Gets the property to type mapping
    /// </summary>
    private (string Name, string InstanceName, object Implementor)[] GetPropertyData<T>(Type detourType, object detour, object fallback)
    {
        var result = new List<(string Name, string instanceName, object Implementor)>();
        var interfaceType = typeof(T);
        
        var propertyInfoArray = GetProperties(interfaceType);
        foreach (var propertyInfo in propertyInfoArray)
        {
            if (detourType.GetProperty(propertyInfo.Name) is { } dp)
            {
                var propertyImplementations = (PropertyImplementationAttribute[])(dp.GetCustomAttributes(typeof(PropertyImplementationAttribute), true));
                foreach (var propertyImplementation in propertyImplementations)
                {
                    if (propertyImplementation is { Name: {} impName, Interface: {} iType} &&
                        dp is { Name : {} name} && detour is not null &&
                        (interfaceType == iType || interfaceType.GetInterfaces().Contains(iType)))
                    {
                        result.Add((impName, name, detour));
                    }
                }
            }
            else
            {
                result.Add((propertyInfo.Name, propertyInfo.Name, fallback));
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Gets the event to type mapping
    /// </summary>
    private (string Name, string InstanceName, object Implementor)[] GetEventData<T>(Type detourType, object detour, object fallback)
    {
        var result = new List<(string Name, string instanceName, object Implementor)>();
        var interfaceType = typeof(T);
        
        var eventInfoArray = GetEvents(interfaceType);
        foreach (var eventInfo in eventInfoArray)
        {
            if (detourType.GetEvent(eventInfo.Name) is { } dp)
            {
                var eventImplementations = (EventImplementationAttribute[])(dp.GetCustomAttributes(typeof(EventImplementationAttribute), true));
                foreach (var eventImplementation in eventImplementations)
                {
                    if (eventImplementation is { Name: { } impName, Interface: { } iType } &&
                        dp is { Name : { } name } && detour is not null &&
                        (interfaceType == iType || interfaceType.GetInterfaces().Contains(iType)))
                    {
                        result.Add((impName, name, detour));
                    }
                }
            }
            else
            {
                result.Add((eventInfo.Name, eventInfo.Name, fallback));
            }
        }

        return result.ToArray();
    }
}
