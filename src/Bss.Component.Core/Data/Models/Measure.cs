using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Models;

public class Measure(
    string name,
    MeasureType type,
    MeasureSource inputSource,
    Guid createdBy,
    bool disabled)
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = name;

    public MeasureType Type { get; private set; } = type;

    public MeasureSource InputSource { get; private set; } = inputSource;

    public string[] Options { get; private set; } = [];

    public bool Disabled { get; private set; } = disabled;

    public Guid CreatedBy { get; private set; } = createdBy;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

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