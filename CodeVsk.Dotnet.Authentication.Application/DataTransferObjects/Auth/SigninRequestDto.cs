using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth
{
    public class SigninRequestDto
    {
        public string Email { get;set; }
        public string Password { get;set; }
    }
}
