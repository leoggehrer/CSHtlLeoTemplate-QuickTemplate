//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Modules.RestApi
{
    using QuickTemplate.Logic.Modules.Exceptions;
    using System.Net.Http.Headers;
    using System.Text.Json;

    public partial class LogonAccess
    {
        protected static string MediaType => "application/json";
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        protected static HttpClient CreateClient(string baseAddress)
        {
            HttpClient client = new();

            if (baseAddress.HasContent())
            {
                if (baseAddress.EndsWith(@"/") == false
                    && baseAddress.EndsWith(@"\") == false)
                {
                    baseAddress += "/";
                }

                client.BaseAddress = new Uri(baseAddress);
            }
            client.DefaultRequestHeaders.Accept.Clear();

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            return client;
        }

        public static async Task<Models.Account.LoginSession?> LogonAsync<T>(string baseAddress, string email, string password, string info)
        {
            using var client = CreateClient(baseAddress);
            var response = await client.GetAsync($"Accounts/Logon/{email}/{password}/{info}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<Models.Account.LoginSession>(contentData, DeserializerOptions).ConfigureAwait(false);

                return result;
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new LogicException(errorMessage);
            }
        }
        public static async Task LogoutAsync(string baseAddress, string sessionToken)
        {
            using var client = CreateClient(baseAddress);
            var response = await client.GetAsync($"Accounts/Logout/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new LogicException(errorMessage);
            }
        }
    }
}
#endif
//MdEnd