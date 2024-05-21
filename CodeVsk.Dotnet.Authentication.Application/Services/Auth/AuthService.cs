using CodeVsk.Dotnet.Authentication.Application.Configurations;
using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using CodeVsk.Dotnet.Authentication.Application.Responses;
using CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces;
using CodeVsk.Dotnet.Authentication.Application.Validators;
using CodeVsk.Dotnet.Authentication.Domain.Contracts.Providers;
using CodeVsk.Dotnet.Authentication.Domain.Utils;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CodeVsk.Dotnet.Authentication.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IRedisProvider _redisProvider;

        private readonly JwtConfigurations _jwtConfiguration;

        public AuthService(IOptions<JwtConfigurations> jwtConfigurations, IRedisProvider redisProvider, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _redisProvider = redisProvider;

            _jwtConfiguration = jwtConfigurations.Value;
        }

        public async Task<Response<SignupResponseDto>> Signup(SignupRequestDto signupRequestDto, CancellationToken cancellationToken)
        {
            SignupRequestValidator validator = new SignupRequestValidator();
            ValidationResult validated = await validator.ValidateAsync(signupRequestDto, cancellationToken);

            if (!validated.IsValid)
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

            if (!createResult.Succeeded)
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

        public async Task<Response<SigninResponseDto>> Signin(SigninRequestDto signinRequestDto, CancellationToken cancellationToken)
        {
            SigninRequestValidator validator = new SigninRequestValidator();
            ValidationResult validated = await validator.ValidateAsync(signinRequestDto, cancellationToken);

            if (!validated.IsValid)
            {
                throw new ValidationException(validated.Errors);
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(signinRequestDto.Username, signinRequestDto.Password, false, true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return new Response<SigninResponseDto>("Essa conta está bloqueada");
                else if (signInResult.IsNotAllowed)
                    return new Response<SigninResponseDto>("Essa conta não tem permissão para fazer login");
                else
                    return new Response<SigninResponseDto>("Usuário ou senha estão incorretos.");
            }

            SigninResponseDto tokenResult = await GenerateToken(signinRequestDto.Username, cancellationToken);

            if (tokenResult == null)
            {
                return new Response<SigninResponseDto>("Ocorreu uma falha ao geração do token.");
            }

            return new Response<SigninResponseDto>(tokenResult);
        }

        public async Task<SigninResponseDto> GenerateToken(string username, CancellationToken cancellationToken)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            HashSet<Claim> claims = await GetClaims(user);

            string roleCache = await CreateRoleCache(user, cancellationToken);

            if (roleCache.IsNullOrEmpty())
            {
                return null;
            }

            DateTime expireAt = DateTime.Now.AddSeconds(_jwtConfiguration.AccessTokenExpiration);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expireAt,
                signingCredentials: _jwtConfiguration.SigningCredentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            SigninResponseDto signinResponseDto = new SigninResponseDto()
            {
                AccessToken = token
            };

            return signinResponseDto;
        }

        private async Task<string> CreateRoleCache(IdentityUser user, CancellationToken cancellationToken)
        {
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            string CACHE_KEY = $"Authentication:{user.Id}";
            string claims_string = claims.ObjectToString<IList<Claim>>();

            string cacheValue = await _redisProvider.SetAsync(CACHE_KEY, claims_string, cancellationToken);

            return cacheValue;
        }

        private async Task<HashSet<Claim>> GetClaims(IdentityUser user)
        {
            HashSet<Claim> claims = new HashSet<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            return claims;
        }
    }
}
