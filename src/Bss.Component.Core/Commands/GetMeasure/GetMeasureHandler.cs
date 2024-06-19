using Bss.Component.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bss.Component.Core.Commands.GetMeasure;

// TODO: Remove all code below, this is just a placeholder

public class GetMeasureHandler(IOptions<BssCoreConfiguration> options, IOptions<BssCore2Configuration> options1)
{
    public Task<GetMeasureResponse> Handle(GetMeasureRequest request)
    {
        return Task.FromResult(new GetMeasureResponse
        {
            MeasureId = options.Value.Test + options1.Value.Test
        });
    }
}

public class SetMeasureHandler
{
    public Task<GetMeasureResponse> Handle()
    {
        throw new NotImplementedException();
    }
}

public class Set1MeasureHandler
{
    public Task Handle(GetMeasureRequest request)
    {
        throw new NotImplementedException();
    }
}

public class TestContrloller: ControllerBase
{

    [HttpGet]
    [Route("api/GetMeasure2")]
    public async Task<IActionResult> GetMeasure2([FromQuery] GetMeasureRequest request)
    {
        return Ok("response");
    }
}