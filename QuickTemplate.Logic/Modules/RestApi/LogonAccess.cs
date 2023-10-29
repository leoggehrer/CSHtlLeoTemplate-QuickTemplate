//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Modules.RestApi
{
    using QuickTemplate.Logic.Modules.Exceptions;
    using System.Net.Http.Headers;
    using System.Text.Json;
    
    /// <summary>
    /// Represents a class that provides access to logon and logout functionality.
    /// </summary>
    public partial class LogonAccess
    {
        /// <summary>
        /// Gets the media type for serialization and deserialization as "application/json".
        /// </summary>
        /// <value>
        /// The media type as a string.
        /// </value>
        protected static string MediaType => "application/json";
        /// <summary>
        /// Gets or sets the options used for deserializing JSON objects.
        /// </summary>
        /// <value>
        /// The deserializer options.
        /// </value>
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        /// <summary>
        /// Creates an instance of the <see cref="HttpClient"/> class and sets the base address and default request headers.
        /// </summary>
        /// <param name="baseAddress">The base address for the HTTP client.</param>
        /// <returns>An initialized instance of the <see cref="HttpClient"/> class.</returns>
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
        
        /// <summary>
        /// Logs in a user asynchronously using the provided email, password and info.
        /// </summary>
        /// <typeparam name="T">The type of the login session object.</typeparam>
        /// <param name="baseAddress">The base address of the API.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="info">Additional information for logging in.</param>
        /// <returns>The login session object if login is successful; otherwise, null.</returns>
        /// <exception cref="LogicException">Thrown when the login fails.</exception>
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
        /// <summary>
        /// Logs out the current session asynchronously.
        /// </summary>
        /// <param name="baseAddress">The base address of the API.</param>
        /// <param name="sessionToken">The session token for the current session.</param>
        /// <returns>A task representing the asynchronous logout operation.</returns>
        /// <exception cref="LogicException">Thrown when the logout operation is not successful.</exception>
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
