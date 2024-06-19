using Bss.Infrastructure;

await Application.Run(
    args, 
    typeof(Bss.Component.Core.Module),
    typeof(Bss.Dal.Module),
    typeof(Bss.Component.Identity.Module),
    typeof(Bss.Dal.Migrations.Module),
    typeof(Bss.Bootstrap.Module));
