namespace Ratcow.DynamicInterface;

public abstract class BaseMapper
{
    /// <summary>
    /// Creates the raw type given the interface and objects provided
    /// </summary>
    protected abstract Type? CreateTypeImplementation<T>(params object[] instances);

    /// <summary>
    /// Creates an instance of the dynamic type for the given interface and objects provided
    /// </summary>
    protected abstract T? CreateInstanceImplementation<T>(params object[] instances);

    /// <summary>
    /// Create a constructor appropriate for this instance
    /// </summary>
    protected abstract void AddConstructor(TypeBuilder typeBuilder, FieldBuilder[] fields);

    /// <summary>
    /// Adds fields that contain the instances passed. We use this to glue the interface to the instances
    /// </summary>
    protected abstract FieldBuilder[] AddFields(TypeBuilder typeBuilder, object[] instances);

    /// <summary>
    /// Code to add a property
    /// </summary>
    protected abstract void AddProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo, (string Name, string InstanceName, object Implementor) instance, FieldBuilder field);

    /// <summary>
    /// Code to add a method
    /// </summary>
    protected abstract void AddMethod(TypeBuilder dynamicType, MethodInfo method, (string Name, string implementorName, object Implementor) methodInstance, FieldBuilder field);

    /// <summary>
    /// Code to add an event
    /// </summary>
    protected abstract void AddEvent(TypeBuilder typeBuilder, EventInfo eventInfo, (string Name, string InstanceName, object Implementor) instance, FieldBuilder field);

    protected static IEnumerable<MethodInfo> GetMethods(Type type)
    {
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
        {
            yield return method;
        }

        if (type.IsInterface)
        {
            foreach (var iface in type.GetInterfaces())
            {
                foreach (var method in GetMethods(iface))
                {
                    yield return method;
                }
            }
        }
    }

    protected static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
        {
            yield return property;
        }

        if (type.IsInterface)
        {
            foreach (var iface in type.GetInterfaces())
            {
                foreach (var property in GetProperties(iface))
                {
                    yield return property;
                }
            }
        }
    }

    protected static IEnumerable<EventInfo> GetEvents(Type type)
    {
        foreach (var eventinfo in type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
        {
            yield return eventinfo;
        }

        if (type.IsInterface)
        {
            foreach (var iface in type.GetInterfaces())
            {
                foreach (var eventinfo in GetEvents(iface))
                {
                    yield return eventinfo;
                }
            }
        }
    }
}