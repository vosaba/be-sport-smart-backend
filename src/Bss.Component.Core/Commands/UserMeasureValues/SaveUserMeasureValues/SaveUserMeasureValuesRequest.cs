using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.UserMeasureValues.SaveUserMeasureValues;

public class SaveUserMeasureValuesRequest
{
    [Required]
    public Dictionary<string, string> Values { get; set; } = [];
}
