using MediatR;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.CreateUser
{
    public sealed record class CreateUserCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password) : IRequest<Result<string>>;
}