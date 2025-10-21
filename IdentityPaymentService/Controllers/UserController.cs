using IdentityPaymentService.Models.User.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPaymentService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAccountDetail([FromRoute] Guid id, [FromBody] UpdateUserAccountDetailRequest request, CancellationToken token)
        {
            var command = request.ToCommand(id);
            await _mediator.Send(command, token);

            return Ok();
        }
    }
}
