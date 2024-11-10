using eAccountingServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace eAccountingServer.Application.Features.Users.GetAllUsers
{
    public sealed record class GetAllUsersQuery() : IRequest<Result<List<AppUser>>>;


}