using Bss.Component.Core.Enums;
using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Component.Core.Configuration;

[Configuration]
public class BssCoreConfiguration
{
    public ComputationEngine DefaultComputationEngine { get; set; }
}
