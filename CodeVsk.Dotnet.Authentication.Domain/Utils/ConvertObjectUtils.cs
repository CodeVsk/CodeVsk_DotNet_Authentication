using System.Text.Json;

namespace CodeVsk.Dotnet.Authentication.Domain.Utils
{
    public static class ConvertObjectUtils
    {
        public static T StringToObject<T>(this string value)
        {
            T jsonValue = JsonSerializer.Deserialize<T>(value);

            return jsonValue;
        }

        public static string ObjectToString<T>(this T value)
        {
            string jsonValue = JsonSerializer.Serialize(value);

            return jsonValue;
        }
    }
}
