namespace Ratcow.DynamicInterface;

public class PropertyImplementationAttribute : Attribute
{
    public Type? Interface { get; set; }
    public string? Name { get; set; }
}