using FCG.Application.Commands;
using FCG.Application.DataContracts.UserDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var token = await mediator.Send(command);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }
        }
    }
}