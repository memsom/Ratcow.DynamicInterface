namespace Ratcow.DynamicInterface;

public static class PropertyInfoExtension
{
    static IEnumerable<PropertyInfo> GetPublicPropertiesImpl(Type type)
    {
        if (!type.IsInterface)
            return type.GetProperties();

        return (new Type[] { type })
            .Concat(type.GetInterfaces())
            .SelectMany(i => i.GetProperties());
    }

    public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
    {
        return GetPublicPropertiesImpl(type);
    }
}