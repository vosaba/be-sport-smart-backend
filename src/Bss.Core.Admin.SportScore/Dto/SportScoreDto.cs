namespace Bss.Core.Admin.SportScore.Dto;

public class SportScoreDto
{
    public string SportName { get; set; } = string.Empty;

    public Dictionary<string, double> SportScoreData { get; set; } = [];
}
