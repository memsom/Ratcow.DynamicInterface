namespace Ratcow.DynamicInterface;

public abstract class BaseMapper
{
    /// <summary>
    /// Creates the raw type given the interface and objects provided
    /// </summary>
    public abstract Type? CreateType<T>(params object[] instances);

    /// <summary>
    /// Creates an instance of the dynamic type for the given interface and objects provided
    /// </summary>
    public abstract T? CreateInstance<T>(params object[] instances);

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

    /// <summary>
    /// Gets the method to type mapping.
    /// </summary>
    protected abstract (string Name, string ImplementorName, object Implementor)[] GetMethodData<T>(object[] instances);

    /// <summary>
    /// Gets the property to type mapping.
    /// </summary>
    protected abstract (string Name, string InstanceName, object Implementor)[] GetPropertyData<T>(object[] instances);

    /// <summary>
    /// Gets the event to type mapping.
    /// </summary>
    protected abstract (string Name, string InstanceName, object Implementor)[] GetEventData<T>(object[] instances);
}