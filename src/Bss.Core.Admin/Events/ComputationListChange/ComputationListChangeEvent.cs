using Bss.Core.Bl.Enums;
using MediatR;

namespace Bss.Core.Admin.Events.ComputationListChange;

internal class ComputationListChangeEvent(ComputationEngine computationEngine) : INotification
{
    public ComputationEngine ComputationEngine => computationEngine;
}