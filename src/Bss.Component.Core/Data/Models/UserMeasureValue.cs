namespace Bss.Component.Core.Data.Models;

public class UserMeasureValue
{
    public UserMeasureValue(Guid userId, string name, string value)
        => (UserId, Name, Value) = (userId, name, value);

    public Guid Id { get; init; }

    public Guid UserId { get; private set; }

    public string Name { get; private set; }

    public string Value { get; private set; }

    public void UpdateValue(string value)
    {
        Value = value;
    }
}
