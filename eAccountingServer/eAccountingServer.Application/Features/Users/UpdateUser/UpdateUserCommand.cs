using MediatR;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.UpdateUser
{
    public sealed record class UpdateUserCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string? Password) : IRequest<Result<string>>;
}