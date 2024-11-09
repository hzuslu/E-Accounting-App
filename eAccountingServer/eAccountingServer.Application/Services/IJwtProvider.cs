using eAccountingServer.Application.Features.Auth.Login;
using eAccountingServer.Domain.Entities;

namespace eAccountingServer.Application.Services
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateToken(AppUser user);
    }
}
