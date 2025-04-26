namespace Bss.Core.Bl.Services;

public interface INotificationService
{
    Task SendNewRequestAdminNotificationAsync(string email, string? phone, IDictionary<string, string> userData);
}
