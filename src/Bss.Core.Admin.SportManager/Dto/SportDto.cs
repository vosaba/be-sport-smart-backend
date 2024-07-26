namespace Bss.Core.Admin.SportManager.Dto;

public class SportDto
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = [];
}
