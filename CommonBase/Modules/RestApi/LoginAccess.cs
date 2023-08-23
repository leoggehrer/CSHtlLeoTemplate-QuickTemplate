//@CodeCopy
//MdStart
#if ACCOUNT_ON
using CommonBase.Extensions;
using CommonBase.Modules.Exceptions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CommonBase.Modules.RestApi
{
    public partial class LoginAccess
    {
        #region static properties
        protected static string MediaType => "application/json";
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        #endregion static properties

        #region static methods
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
        public static async Task<T?> LogonAsync<T>(string baseAddress, string email, string password, string info)
        {
            using var client = CreateClient(baseAddress);
            var model = new Models.Accounts.Logon { Email = email, Password = password, Info = info };
            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync($"Accounts/Logon", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<T>(contentData, DeserializerOptions).ConfigureAwait(false);

                return result;
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
        }
        public static async Task LogoutAsync(string baseAddress, string sessionToken)
        {
            using var client = CreateClient(baseAddress);
            var jsonData = JsonSerializer.Serialize(sessionToken);
            var content = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PutAsync($"Accounts/Logout", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
        }
        public static async Task<bool> IsSessionAlive(string baseAddress, string sessionToken)
        {
            var result = false;
            using var client = CreateClient(baseAddress);
            var response = await client.GetAsync($"Accounts/IsSessionAlive/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                result = Convert.ToBoolean(contentData);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result;
        }
        #endregion static methods
    }
}
#endif
//MdEnd
