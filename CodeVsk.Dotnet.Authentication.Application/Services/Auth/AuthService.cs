using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using CodeVsk.Dotnet.Authentication.Application.Responses;
using CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces;
using CodeVsk.Dotnet.Authentication.Application.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace CodeVsk.Dotnet.Authentication.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(UserManager<IdentityUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<Response<SignupResponseDto>> Signup(SignupRequestDto signupRequestDto, CancellationToken cancellationToken)
        {
            SignupRequestValidator validator = new SignupRequestValidator();
            ValidationResult validated = await validator.ValidateAsync(signupRequestDto, cancellationToken);

            if(!validated.IsValid)
            {
                throw new ValidationException(validated.Errors);
            }

            IdentityUser identityUser = new IdentityUser()
            {
                UserName = signupRequestDto.Name,
                Email = signupRequestDto.Email,
                EmailConfirmed = true
            };

            IdentityResult createResult = await _userManager.CreateAsync(identityUser, signupRequestDto.Password);

            if(!createResult.Succeeded)
            {
                return new Response<SignupResponseDto>(createResult.Errors);
            }

            await _userManager.SetLockoutEnabledAsync(identityUser, false);

            SignupResponseDto signupResponseDto = new SignupResponseDto()
            {
                Email = signupRequestDto.Email,
                Name = signupRequestDto.Name
            };

            return new Response<SignupResponseDto>(signupResponseDto);
        }
    }
}
