using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Models;

public class Measure
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public MeasureType Type { get; private set; }

    public MeasureSource InputSource { get; private set; }

    public string[] Options { get; private set; } = [];

    public bool Disabled { get; private set; }

    public string CreatedBy { get; init; } = string.Empty;

    public required DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; private set; }

    public static Measure Create(
        string name,
        MeasureType type,
        MeasureSource inputSource,
        string createdBy)
    {
        return new Measure
        {
            Name = name,
            Type = type,
            InputSource = inputSource,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string name,
        MeasureType type,
        MeasureSource inputSource,
        string[] options,
        bool disabled)
    {
        Name = name;
        Type = type;
        InputSource = inputSource;
        Options = options;
        Disabled = disabled;
        UpdatedAt = DateTime.UtcNow;
    }
}