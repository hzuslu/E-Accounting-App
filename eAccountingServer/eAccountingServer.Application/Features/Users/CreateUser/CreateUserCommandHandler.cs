using AutoMapper;
using eAccountingServer.Domain.Entities;
using eAccountingServer.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.CreateUser
{
    internal sealed class CreateUserCommandHandler(
        IMediator mediatr,
        UserManager<AppUser> userManager,
        IMapper mapper) : IRequestHandler<CreateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isUsernameExist = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
            if (isUsernameExist)
                return Result<string>.Failure("This username is already using");

            bool isEmailExist = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);
            if (isEmailExist)
                return Result<string>.Failure("This email is already using");

            AppUser appUser = mapper.Map<AppUser>(request);
            IdentityResult identityResult = await userManager.CreateAsync(appUser, request.Password);

            if (!identityResult.Succeeded)
                return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());

            await mediatr.Publish(new AppUserEvent(appUser.Id));
            return "User created successfully";

        }
    }
}