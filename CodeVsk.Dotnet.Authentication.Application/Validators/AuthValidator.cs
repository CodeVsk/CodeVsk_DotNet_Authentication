using CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth;
using FluentValidation;

namespace CodeVsk.Dotnet.Authentication.Application.Validators
{
    public class SignupRequestValidator : AbstractValidator<SignupRequestDto>
    {
        public SignupRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Password).MinimumLength(5).WithMessage("A senha deve conter no minimo 5 caracteres.");
        }
    }

    public class SigninRequestValidator : AbstractValidator<SigninRequestDto>
    {
        public SigninRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("O nome de usuário é obrigatório.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("A senha é obrigatório.").MinimumLength(5).WithMessage("A senha deve conter no minimo 5 caracteres.");
        }
    }
}
