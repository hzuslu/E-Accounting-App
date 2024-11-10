using MediatR;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.DeleteUserById
{
    public sealed record class DeleteUserByIdCommand(
        Guid Id) : IRequest<Result<string>>;
}