using eAccountingServer.Domain.Entities;
using eMuhasebeServer.Application.Features.Auth.Login;

namespace eMuhasebeServer.Application.Services;
public interface IJwtProvider
{
    Task<LoginCommandResponse> CreateToken(AppUser user);
}