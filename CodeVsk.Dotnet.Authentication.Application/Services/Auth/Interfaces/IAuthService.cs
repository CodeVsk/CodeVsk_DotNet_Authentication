using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using CodeVsk.Dotnet.Authentication.Application.Responses;

namespace CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Response<SigninResponseDto>> Signin(SigninRequestDto signinRequestDto, CancellationToken cancellationToken);
        Task<Response<SignupResponseDto>> Signup(SignupRequestDto signupRequestDto, CancellationToken cancellationToken);
    }
}
