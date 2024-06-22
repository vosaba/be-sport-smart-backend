using Bss.Component.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Bss.Component.Core.Commands.GetMeasure;

public class GetMeasureHandler(IOptions<BssCoreConfiguration> options)
{
    public Task<GetMeasureResponse> Handle(GetMeasureRequest request)
    {
        throw new NotImplementedException();
    }
}
