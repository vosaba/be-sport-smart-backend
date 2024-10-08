using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Core.Engine.Configuration;

public enum MeasureRequirementsCheckMode
{
    Direct,
    Inherited,
    None
}

[Configuration]
public class CoreEngineConfiguration
{
    public MeasureRequirementsCheckMode MeasureRequirementsCheckMode { get; set; } = MeasureRequirementsCheckMode.Direct;
}
