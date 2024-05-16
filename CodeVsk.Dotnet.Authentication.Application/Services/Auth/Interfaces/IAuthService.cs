using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using CodeVsk.Dotnet.Authentication.Application.Responses;
using System.Security.Claims;

namespace CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Response<SignupResponseDto>> Signup(SignupRequestDto signupRequestDto, CancellationToken cancellationToken);

        Task<HashSet<Claim>> GenerateToken(string email);
    }
}
