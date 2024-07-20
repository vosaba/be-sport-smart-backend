using Bss.Core.Bl.Enums;

namespace Bss.Core.Engine.Services.MeasureValues;

public record MeasureValue(string Name, MeasureType Type, string Value);