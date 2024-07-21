using Bss.Core.Bl.Enums;
using MediatR;

namespace Bss.Core.Admin.Events;

public class ComputationListChangeEvent(ComputationEngine computationEngine) : INotification
{
    public ComputationEngine ComputationEngine => computationEngine;
}