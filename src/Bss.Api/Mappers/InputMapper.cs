using Bss.Api.Data.Models;
using Bss.Api.Dtos.Input;

namespace Bss.Api.Mappers
{
    public static class InputMapper
    {
        public static InputDto ToInputDto(this Input input)
        {
            return new InputDto
            {
                Name = input.Name,
                Type = input.Type,
                InputSource = input.InputSource,
                Options = input.Options
            };
        }
    }
}