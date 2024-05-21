using Microsoft.AspNetCore.Identity;

namespace CodeVsk.Dotnet.Authentication.Application.Responses
{
    public class Response<T>
    {
        public T Values { get; set; }
        public string Status { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();

        public Response(IEnumerable<IdentityError> errors) 
        {
            Status = "error";
            Errors = errors.Select(x => x.Description).ToList();
        }

        public Response(T values)
        {
            Status = "success";
            Values = values;
        }

        public Response(string message)
        {
            Status = "error";
            Errors.Add(message);
        }
    }
}
