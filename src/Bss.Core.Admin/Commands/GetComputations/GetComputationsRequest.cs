using Bss.Core.Bl.Enums;

namespace Bss.Component.Core.Commands.Admin.GetComputations;

public class GetComputationsRequest
{
    public string? Name { get; init; }

    public ComputationType? Type { get; init; }

    public ComputationEngine? Engine { get; init; }

    public bool? Disabled { get; init; }
}
