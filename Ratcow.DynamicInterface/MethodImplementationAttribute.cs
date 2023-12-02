namespace Ratcow.DynamicInterface;

public class MethodImplementationAttribute : Attribute
{
    public Type? Interface { get; set; }
    public string? Name { get; set; }
}