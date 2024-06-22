using Bss.Component.Core.Enums;
using MediatR;

namespace Bss.Component.Core.Events.ComputationListChange;

internal class ComputationListChangeEvent(ComputationEngine computationEngine) : INotification
{
    public ComputationEngine ComputationEngine => computationEngine;
}