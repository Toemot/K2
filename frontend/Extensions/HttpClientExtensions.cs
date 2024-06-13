using System.Text.Json;

namespace frontend.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API {responseMessage.StatusCode} {responseMessage.ReasonPhrase}");

            var dataAsString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
