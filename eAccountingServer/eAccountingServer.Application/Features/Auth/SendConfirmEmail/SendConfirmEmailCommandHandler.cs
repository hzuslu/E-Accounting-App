using eAccountingServer.Domain.Entities;
using eAccountingServer.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace eAccountingServer.Application.Features.Auth.SendConfirmEmail
{
    internal sealed record SendConfirmEmailCommandHandler(
        UserManager<AppUser> _userManager,
        IMediator mediator) : IRequestHandler<SendConfirmEmailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(SendConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<string>.Failure("The provided email address was not found in the database.");

            if (user.EmailConfirmed)
                return Result<string>.Failure("This email address is already confirmed.");

            await mediator.Publish(new AppUserEvent(user.Id));
            return Result<string>.Succeed("The email address has been successfully confirmed.");
        }
    }
}