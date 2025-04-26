using Bss.Core.Bl.Services;
using Bss.SendGrid.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Bss.SendGrid.Services;

internal class NotificationService(ISendGridClient sendGridClient, IOptions<SendGridConfiguration> options, ILogger<NotificationService> logger) 
    : INotificationService
{
    private readonly ISendGridClient _sendGridClient = sendGridClient;
    private readonly SendGridConfiguration _configuration = options.Value;
    private readonly ILogger _logger = logger;   

    public async Task SendNewRequestAdminNotificationAsync(string email, string zip, string name, string? phone, IDictionary<string, string>? userData)
    {
        const string templateId = "d-100c5b608f444eba95ad0198ac03c3fe"; // TODO: move to config (non-sensative)
        var templateData = new
        {
            userEmail = email,
            userPhone = phone,
            userName = name,
            userZip = zip,
            userData
        };

        await SendSendGridEmailAsync(_configuration.AdminEmail, templateId, templateData);
    }

    private async Task SendSendGridEmailAsync (string toEmail, string templateId, object templateData)
    {
        try
        {
            var from = new EmailAddress(_configuration.FromEmail, _configuration.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);

            await _sendGridClient.SendEmailAsync(msg);

            _logger.LogInformation("Email sent to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
        }
    }
}
