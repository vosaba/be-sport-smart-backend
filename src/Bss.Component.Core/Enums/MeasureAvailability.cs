namespace Bss.Component.Core.Enums;

[Flags]
public enum MeasureAvailability
{
    NoRestriction = 1,
    User = 2,
    UserReadonly = 4,
    Facility = 8,
}
