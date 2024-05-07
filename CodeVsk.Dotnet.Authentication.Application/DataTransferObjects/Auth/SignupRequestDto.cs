using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsk.Dotnet.Authentication.Application.DataTransferObjects.Auth
{
    public class SignupRequestDto
    {
        public string Name { get;set; }
        public string Email { get;set; }
        public string Password { get;set; }
    }
}
