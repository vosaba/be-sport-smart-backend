using Bss.Core.Bl.Enums;

namespace Bss.Core.Bl.Models;

public class Measure(
    string name,
    MeasureType type,
    double? minValue,
    double? maxValue,
    MeasureAvailability availability,
    Guid createdBy,
    bool disabled,
    string[] options,
    int order)
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = name;

    public MeasureType Type { get; private set; } = type;

    public double? MinValue { get; private set; } = minValue;

    public double? MaxValue { get; private set; } = maxValue;

    public int Order { get; private set; } = order;

    public MeasureAvailability Availability { get; private set; } = availability;

    public string[] Options { get; private set; } = options;

    public bool Disabled { get; private set; } = disabled;

    public Guid CreatedBy { get; private set; } = createdBy;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void Update(
        string name,
        MeasureType type,
        double? minValue,
        double? maxValue,
        MeasureAvailability availability,
        string[] options,
        bool disabled,
        int order)
    {
        Name = name;
        Type = type;
        MinValue = minValue;
        MaxValue = maxValue;
        Availability = availability;
        Options = options;
        Disabled = disabled;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsVisibleForUser(bool isSignedIn)
        => !Disabled
        && (Availability == MeasureAvailability.NoRestriction
            || isSignedIn && (Availability == MeasureAvailability.User || Availability == MeasureAvailability.UserReadonly));

    public bool IsMeasurableByUser(bool isSignedIn)
        => !Disabled
        && (Availability == MeasureAvailability.NoRestriction || isSignedIn && Availability == MeasureAvailability.User);
}