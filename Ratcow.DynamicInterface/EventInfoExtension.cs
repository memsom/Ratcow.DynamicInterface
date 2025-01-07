namespace Ratcow.DynamicInterface;

public static class EventInfoExtension
{
    static IEnumerable<EventInfo> GetEventsImpl(Type type)
    {
        if (!type.IsInterface)
            return type.GetEvents();

        return (new[] { type })
            .Concat(type.GetInterfaces())
            .SelectMany(i => i.GetEvents());
    }

    public static IEnumerable<EventInfo> GetPublicEvents(this Type type)
    {
        return GetEventsImpl(type);
    }
}
