using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace User.Adapters.Inbound.Api.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Test(CancellationToken cancellationToken)
        {
            return Ok("test");
        }
    }
}
