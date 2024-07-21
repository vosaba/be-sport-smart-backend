using Bss.Infrastructure;

await Application.Run(
    args, 
    typeof(Bss.Core.Admin.Module),
    typeof(Bss.Core.Admin.SportScore.Module),
    typeof(Bss.Core.Engine.Module),
    typeof(Bss.UserValues.Module),
    typeof(Bss.Identity.Module),
    typeof(Bss.Dal.Module),
    typeof(Bss.Dal.Migrations.Module));
