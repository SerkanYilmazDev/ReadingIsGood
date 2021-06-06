using System.Threading.Tasks;
using Services.Identity.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Services.Identity.Commands;
using Services.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Identity.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IMediator _mediator;
        private readonly IdentityDBContext _dbContext;

        public AccountsController(IAuthenticationService authService,
                IMediator mediator,
                IdentityDBContext dbContext)
        {
            _dbContext = dbContext;
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInDto reqeust)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(s => s.Email == reqeust.Email && s.Password == reqeust.Password);

            if (user == null)
                return BadRequest("Invalid Credentials.");

            return Ok(_authService.GetToken(user));
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}