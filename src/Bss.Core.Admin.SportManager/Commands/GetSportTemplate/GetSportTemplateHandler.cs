using Bss.Core.Admin.SportManager.Dto;
using Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Core.Admin.SportManager.Commands.GetSportTemplate;

[Authorize(Roles = "Admin")]
public class GetSportTemplateHandler(
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    private const string _newSportName = "new_sport";
    public SportDto Handle(GetSportTemplateRequest request)
    {
        var sportFormulaManipulator = sportFormulaManipulatorFactory.GetService(request.Engine);
        var sportFormula = sportFormulaManipulator.CreateFormulaUsingData([]);

        return new SportDto
        {
            Name = string.Empty,
            Variables = sportFormulaManipulator.GetFormulaVariables(sportFormula),
            Formula = sportFormula,
        };
    }
}
