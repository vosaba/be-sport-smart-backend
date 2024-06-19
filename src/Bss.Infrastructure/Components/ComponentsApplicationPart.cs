using Bss.Infrastructure.Shared;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace Bss.Infrastructure.Components;

internal class ComponentsApplicationPart : ApplicationPart, IApplicationPartTypeProvider
{
    public override string Name => "Components";

    public IEnumerable<TypeInfo> Types
    {
        get
        {
            foreach (var a in AssemblyManager.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    yield return t.GetTypeInfo();
                }
            }
        }
    }
}