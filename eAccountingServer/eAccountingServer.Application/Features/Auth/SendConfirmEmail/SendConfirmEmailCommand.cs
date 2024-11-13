using MediatR;
using TS.Result;

namespace eAccountingServer.Application.Features.Auth.SendConfirmEmail
{
    public sealed record class SendConfirmEmailCommand(string Email) : IRequest<Result<string>>;
}