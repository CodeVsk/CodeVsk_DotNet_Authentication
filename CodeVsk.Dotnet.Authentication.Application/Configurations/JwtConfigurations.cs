using Microsoft.IdentityModel.Tokens;

namespace CodeVsk.Dotnet.Authentication.Application.Configurations
{
    public class JwtConfigurations
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public int Expiration { get; set; }
    }
}
