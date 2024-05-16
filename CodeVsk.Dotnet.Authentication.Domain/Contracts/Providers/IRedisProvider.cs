using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsk.Dotnet.Authentication.Domain.Contracts.Providers
{
    public interface IRedisProvider
    {
        Task<string> GetAsync(string key, CancellationToken cancellationToken);
        Task<string> SetAsync(string key, string value, CancellationToken cancellationToken);
    }
}
