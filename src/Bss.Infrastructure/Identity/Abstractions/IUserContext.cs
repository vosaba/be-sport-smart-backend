﻿namespace Bss.Infrastructure.Identity.Abstractions;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    string? UserName { get; }

    string? Email { get; }

    IList<string> Roles { get; }

    bool IsInRole(string role);
}
