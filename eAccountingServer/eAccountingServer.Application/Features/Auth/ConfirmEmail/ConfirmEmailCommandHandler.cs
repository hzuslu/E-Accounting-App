using eAccountingServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace eAccountingServer.Application.Features.Auth.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler(UserManager<AppUser> _userManager) : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<string>.Failure("The provided email address was not found in the database.");

            if (user.EmailConfirmed)
                return Result<string>.Failure("This email address is already confirmed.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Result<string>.Succeed("The email address has been successfully confirmed.");
        }
    }


}