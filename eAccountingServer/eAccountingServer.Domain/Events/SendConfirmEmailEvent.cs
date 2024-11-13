using eAccountingServer.Domain.Entities;
using FluentEmail.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eAccountingServer.Domain.Events
{
    public sealed class SendConfirmEmailEvent(
        UserManager<AppUser> _userManager,
        IFluentEmail _fluentEmail) : INotificationHandler<AppUserEvent>
    {
        public async Task Handle(AppUserEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(notification.UserId.ToString());
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                var emailBody = $@"
                                <h1>Email Confirmation</h1>
                                <p>Please confirm your email address by clicking the link below:</p>
                                <a href='http://localhost:4200/confirm-email/{Uri.EscapeDataString(user.Email)}' target='_blank'>Confirm Email</a>";

                await _fluentEmail
                    .To(user.Email)
                    .Subject("Email Confirmation")
                    .Body(emailBody, true)
                    .SendAsync();
            }

        }
    }
}
