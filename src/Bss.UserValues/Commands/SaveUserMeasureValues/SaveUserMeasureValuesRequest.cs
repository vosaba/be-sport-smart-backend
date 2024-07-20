using System.ComponentModel.DataAnnotations;

namespace Bss.UserValues.Commands.UserMeasureValues.SaveUserMeasureValues;

public class SaveUserMeasureValuesRequest
{
    [Required]
    public Dictionary<string, string> Values { get; set; } = [];
}