namespace Ratcow.DynamicInterface;

public class EventImplementationAttribute : Attribute
{
    public Type? Interface { get; set; }
    public string? Name { get; set; }
}