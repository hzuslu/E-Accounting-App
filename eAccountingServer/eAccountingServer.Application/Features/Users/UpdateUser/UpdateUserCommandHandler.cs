using AutoMapper;
using eAccountingServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.UpdateUser
{
    internal sealed class UpdateUserCommandHandler(
        UserManager<AppUser> userManager,
        IMapper mapper) : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcıyı ID ile bulma
            var appUser = await userManager.FindByIdAsync(request.Id.ToString());
            if (appUser == null)
                return Result<string>.Failure("User not found");

            // Kullanıcı adı değişmişse, yeni kullanıcı adının zaten kullanılıp kullanılmadığını kontrol et
            if (appUser.UserName != request.UserName)
            {
                bool isUsernameExist = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
                if (isUsernameExist)
                    return Result<string>.Failure("This username is already taken");
            }

            // E-posta adresi değişmişse, yeni e-posta adresinin zaten kullanılıp kullanılmadığını kontrol et
            bool isMailChanged = false;
            if (appUser.Email != request.Email)
            {
                bool isEmailExist = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);
                if (isEmailExist)
                    return Result<string>.Failure("This email is already in use");

                // E-posta adresi değiştiği için e-posta onayı sıfırlanacak
                isMailChanged = true;
                appUser.EmailConfirmed = false;
            }

            // Kullanıcıyı güncelle
            mapper.Map(request, appUser);
            var updateResult = await userManager.UpdateAsync(appUser);

            if (!updateResult.Succeeded)
                return Result<string>.Failure(updateResult.Errors.Select(e => e.Description).ToList());

            if (request.Password is not null)
            {
                string resetToken = await userManager.GeneratePasswordResetTokenAsync(appUser);
                var resetResult = await userManager.ResetPasswordAsync(appUser, resetToken, request.Password);
                if (!resetResult.Succeeded)
                    return Result<string>.Failure(resetResult.Errors.Select(e => e.Description).ToList());
            }

            // E-posta değişmişse, onay e-postası gönderme işlemi
            if (isMailChanged)
            {
                // Onay maili gönderme kodları burada olacak
            }

            return Result<string>.Succeed("User updated successfully");
        }

    }
}