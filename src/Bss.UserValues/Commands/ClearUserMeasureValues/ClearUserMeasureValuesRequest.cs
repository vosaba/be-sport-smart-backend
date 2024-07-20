namespace Bss.UserValues.Commands.UserMeasureValues.ClearUserMeasureValues;

public class ClearUserMeasureValuesRequest
{
    public string[] Names { get; set; } = [];

    public bool ClearAll { get; set; }
}