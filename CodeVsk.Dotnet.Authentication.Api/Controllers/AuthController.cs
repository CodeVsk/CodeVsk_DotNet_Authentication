using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using CodeVsk.Dotnet.Authentication.Application.Responses;
using CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace codevsk.dotnet.authentication.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;

            _authService = authService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<Response<SignupResponseDto>>> Signup([FromBody] SignupRequestDto signupRequestDto, CancellationToken cancellationToken)
        {
            Response<SignupResponseDto> signupResponseDto = await _authService.Signup(signupRequestDto, cancellationToken);

            return signupResponseDto;
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult<dynamic>> Signup([FromBody] string email)
        {
            var result = await _authService.GenerateToken(email);

            return result;
        }
    }
}
