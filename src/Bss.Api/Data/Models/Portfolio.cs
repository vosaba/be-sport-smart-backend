using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bss.Api.Data.Models
{
    public class Portfolio
    {
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = new AppUser();
    }
}