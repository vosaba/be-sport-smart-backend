using Bss.Core.Bl.Services;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Commands.Users.RequestAssessment;

[Authorize(Roles = "User")]
public class RequestAssessmentHandler(
    ILogger<RequestAssessmentHandler> logger,
    INotificationService notificationService,
    IUserContext userContex)
{
    public async Task Handle(RequestAssessmentRequest request)
    {
        if (request.MeasureValues.Count == 0)
        {
            logger.LogWarning("RequestAssessmentHandler: MeasureValues is empty.");
        }

        logger.LogTrace("User with email {Email} requested assessment for zip code {ZipCode}.", userContex.Email, request.Zip);

        await notificationService.SendNewRequestAdminNotificationAsync(
            userContex.Email!,
            request.Zip,
            request.Name,
            request.Phone,
            request.MeasureValues);
    }
}
