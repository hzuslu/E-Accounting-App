using eAccountingServer.WebAPI.Abstractions;
using FluentEmail.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAccountingServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class TestController : ApiController
    {
        private readonly IFluentEmail _fluentEmail;
        public TestController(IMediator mediator, IFluentEmail fluentEmail) : base(mediator)
        {
            this._fluentEmail = fluentEmail;
        }

        [HttpGet]
        public async Task<IActionResult> SendTestEmail()
        {
            await _fluentEmail
                .To("hasanuslu0278@gmail.com")
                .Subject("Test")
                .Body("<h1>Test Mail Gönderme</h1>", true)
                .SendAsync();

            return NoContent();
        }
    }
}
