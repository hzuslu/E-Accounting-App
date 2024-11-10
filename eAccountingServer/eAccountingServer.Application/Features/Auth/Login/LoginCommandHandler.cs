using eAccountingServer.Application.Services;
using eAccountingServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAccountingServer.Application.Features.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IJwtProvider jwtProvider;

        public LoginCommandHandler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtProvider jwtProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtProvider = jwtProvider;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Attempt to find user by email or username
            var user = await userManager.Users
                .FirstOrDefaultAsync(p =>
                    p.UserName == request.EmailOrUserName ||
                    p.Email == request.EmailOrUserName,
                    cancellationToken);

            if (user == null)
                return Result<LoginCommandResponse>.Failure("User not found");

            // Check sign-in credentials
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            // Account is locked out after multiple failed login attempts
            if (signInResult.IsLockedOut)
            {
                var timeSpan = user.LockoutEnd - DateTime.UtcNow;
                if (timeSpan.HasValue)
                {
                    return Result<LoginCommandResponse>.Failure(
                        $"Your account has been locked for {Math.Ceiling(timeSpan.Value.TotalMinutes)} minutes due to multiple failed login attempts");
                }
                else
                {
                    return Result<LoginCommandResponse>.Failure("Your account has been locked for 5 minutes due to multiple failed login attempts");
                }
            }

            // Account is not allowed to log in (e.g., email is not confirmed)
            if (signInResult.IsNotAllowed)
                return Result<LoginCommandResponse>.Failure("Your email address is not verified");

            // Invalid password
            if (!signInResult.Succeeded)
                return Result<LoginCommandResponse>.Failure("Invalid password");

            // Generate JWT token for successful login
            var loginResponse = await jwtProvider.CreateToken(user);

            return Result<LoginCommandResponse>.Succeed(loginResponse);
        }
    }
}
