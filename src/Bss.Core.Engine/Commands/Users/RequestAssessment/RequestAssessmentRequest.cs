using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Engine.Commands.Users.RequestAssessment;

public class RequestAssessmentRequest
{
    public string? Phone { get; set; }

    [Required]
    public string Zip { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, string> MeasureValues { get; set; } = [];
}
