using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Engine.Commands.Users.RequestAssessment;

public class RequestAssessmentRequest
{
    public string? Phone { get; set; }

    [Required]
    public Dictionary<string, string> MeasureValues { get; set; } = [];
}
