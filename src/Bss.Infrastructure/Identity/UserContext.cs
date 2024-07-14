using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bss.Infrastructure.Identity;

internal class UserContext(IHttpContextAccessor httpContextAccessor) 
    : IUserContext
{
    private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => _claimsPrincipal?.Identity?.IsAuthenticated ?? false;

    public Guid UserId => Guid.Parse(_claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier) 
        ?? throw new InvalidOperationException("User id claim is missing."));

    public string? UserName => _claimsPrincipal?.FindFirstValue(ClaimTypes.Name);

    public string? Email => _claimsPrincipal?.FindFirstValue(ClaimTypes.Email);

    public IList<string> Roles => _claimsPrincipal?
        .FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? throw new InvalidOperationException("User roles claim is missing.");

    public bool IsInRole(string role) => _claimsPrincipal?
        .IsInRole(role) ?? throw new InvalidOperationException("User roles claim is missing.");
}
