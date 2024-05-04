using Bss.Api.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Bss.Api.Dtos.ScoreProvider
{
    public class ScoreProviderDto
    {
        [Required]
        public ScoreProviderType Type { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Formula { get; set; } = string.Empty;
        /* 
            () => { 
                return measures.absolute_bmi() + parameters.kick;
            };
            (param) => { 
                param + measures.absolute_high_20_score(2) + parameters.param1;
            };
         */
    }
}
