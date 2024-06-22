namespace Bss.Infrastructure.Jobs.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class JobAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}