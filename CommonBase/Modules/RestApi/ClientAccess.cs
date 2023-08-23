//@CodeCopy
//MdStart
using CommonBase.Extensions;
using CommonBase.Modules.Exceptions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CommonBase.Modules.RestApi
{
    public partial class ClientAccess
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
        protected static HttpClient CreateClient(string baseAddress, string sessionToken)
        {
            HttpClient client = CreateClient(baseAddress);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{sessionToken}")));

            return client;
        }
        #endregion static methods

        #region properties
        public string BaseAddress { get; init; }
        public string SessionToken { get; init; }
        #endregion properties

        #region constructors
        public ClientAccess(string baseAddress)
            : this(baseAddress, string.Empty)
        {
        }
        public ClientAccess(string baseAddress, string sessionToken)
        {
            BaseAddress = baseAddress;
            SessionToken = sessionToken;
        }
        #endregion constructors

        #region methods
        protected HttpClient CreateClient()
        {
            HttpClient result;

            if (SessionToken.HasContent())
            {
                result = CreateClient(BaseAddress, SessionToken);
            }
            else
            {
                result = CreateClient(BaseAddress);
            }
            return result;
        }

        public async Task<int> GetMaxPageSizeAsync(string requestUri)
        {
            int result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/MaxPageSize").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                result = Convert.ToInt32(contentData);
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

        public async Task<int> GetCountAsync(string requestUri)
        {
            int result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/Count").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                result = Convert.ToInt32(contentData);
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
        public async Task<int> GetCountAsync(string requestUri, string predicate)
        {
            int result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/CountBy/{predicate}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                result = Convert.ToInt32(contentData);
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

#if GUID_ON
        public async Task<T?> GetByGuidAsync<T>(string requestUri, Guid id)
        {
            T? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/ByGuid/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T>(contentData, DeserializerOptions).ConfigureAwait(false);
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
#endif
        public async Task<T?> GetByIdAsync<T>(string requestUri, IdType id)
        {
            T? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T>(contentData, DeserializerOptions).ConfigureAwait(false);
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

        public async Task<T[]> GetAsync<T>(string requestUri)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> GetAsync<T>(string requestUri, string orderBy)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/Sorted/{orderBy}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> GetPageListAsync<T>(string requestUri, int index, int size)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/GetPage/{index}/{size}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> GetPageListAsync<T>(string requestUri, string orderBy, int index, int size)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/GetSortedPage/{orderBy}/{index}/{size}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }

        public async Task<T[]> QueryAllAsync<T>(string requestUri, string predicate)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/Query/{predicate}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> QueryAllAsync<T>(string requestUri, string predicate, string orderBy)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/QuerySorted/{predicate}/{orderBy}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> QueryAllAsync<T>(string requestUri, string predicate, int index, int size)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/QueryPage/{predicate}/{index}/{size}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        public async Task<T[]> QueryAllAsync<T>(string requestUri, string predicate, string orderBy, int index, int size)
        {
            T[]? result;
            using var client = CreateClient();
            var response = await client.GetAsync($"{requestUri}/QuerySortedPage/{predicate}/{orderBy}/{index}/{size}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T[]>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }

        public async Task<T> PostAsync<T>(string requestUri, T model)
        {
            model.CheckArgument(nameof(model));

            T? result;
            using var client = CreateClient();
            var jsonData = JsonSerializer.Serialize(model);
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync($"{requestUri}", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T>(resultData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result!;
        }

        public async Task<T> PutAsync<T>(string requestUri, IdType id, T model)
        {
            model.CheckArgument(nameof(model));

            T? result;
            using var client = CreateClient();
            var jsonData = JsonSerializer.Serialize(model);
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            HttpResponseMessage response = await client.PutAsync($"{requestUri}/{id}", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                result = await JsonSerializer.DeserializeAsync<T>(resultData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                var errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
            return result!;
        }

        public async Task DeleteAsync(string requestUri, IdType id)
        {
            using var client = CreateClient();
            var response = await client.DeleteAsync($"{requestUri}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                var errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new RestApiException(errorMessage);
            }
        }
        #endregion methods
    }
}
//MdEnd
