//@BaseCode
//MdStart
using QuickTemplate.Logic.Modules.Exceptions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuickTemplate.Logic.Modules.RestApi
{
    /// <summary>
    /// Represents a client access class for making HTTP requests to a specified base address with optional session token authentication.
    /// </summary>
    public partial class ClientAccess
    {
        /// <summary>
        /// Gets the media type for the property.
        /// </summary>
        /// <returns>The media type ("application/json").</returns>
        protected static string MediaType => "application/json";
        /// <summary>
        /// Gets the JsonSerializerOptions used for deserialization.
        /// </summary>
        /// <value>
        /// The JsonSerializerOptions used for deserialization.
        /// </value>
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        /// <summary>
        /// Creates and configures an instance of <see cref="HttpClient"/> with the specified base address.
        /// </summary>
        /// <param name="baseAddress">The base address of the HTTP client.</param>
        /// <returns>An instance of <see cref="HttpClient"/>.</returns>*/
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
        /// Creates an instance of the HttpClient class with a specified base address and session token.
        /// </summary>
        /// <param name="baseAddress">The base address of the HTTP client.</param>
        /// <param name="sessionToken">The session token used for authentication.</param>
        /// <returns>An instance of the HttpClient class.</returns>
        protected static HttpClient CreateClient(string baseAddress, string sessionToken)
        {
            HttpClient client = CreateClient(baseAddress);

            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{sessionToken}")));

            return client;
        }

        /// <summary>
        /// Gets or sets the base address for the communication.
        /// </summary>
        /// <value>
        /// A string representing the base address for the communication.
        /// </value>
        public string BaseAddress { get; init; }
        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        public string SessionToken { get; init; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAccess"/> class with the specified base address.
        /// </summary>
        /// <param name="baseAddress">The base address to use for the client access.</param>
        /// <remarks>
        /// This constructor internally calls the parameterized constructor with the base address and empty string for the token.
        /// </remarks>
        public ClientAccess(string baseAddress)
        : this(baseAddress, string.Empty)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAccess"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address of the client.</param>
        /// <param name="sessionToken">The session token to authenticate the client.</param>
        public ClientAccess(string baseAddress, string sessionToken)
        {
            BaseAddress = baseAddress;
            SessionToken = sessionToken;
        }

        /// <summary>
        /// Creates a new instance of the HttpClient class.
        /// </summary>
        /// <returns>
        /// The newly created HttpClient instance.
        /// </returns>
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

        /// <summary>
        /// Retrieves the maximum page size asynchronously.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The maximum page size.</returns>
        /// <exception cref="LogicException">Thrown when the response indicates failure.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves the count from the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The count as an integer.</returns>
        /// <exception cref="LogicException">Thrown when the response status code is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result;
        }
        /// <summary>
        /// Retrieves the count asynchronously from the specified request URI based on the given predicate.
        /// </summary>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="predicate">The predicate to filter the count.</param>
        /// <returns>The count as an integer.</returns>
        /// <exception cref="LogicException">Thrown when the response is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result;
        }

#if GUID_ON
        /// <summary>
        /// Retrieves an entity by a specified Guid asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="id">The Guid identifier of the entity to retrieve.</param>
        /// <returns>
        /// The retrieved entity of type <typeparamref name="T"/> if successful, otherwise null.
        /// </returns>
        /// <exception cref="LogicException">
        /// Thrown when the response from the server indicates an unsuccessful operation.
        /// </exception>
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
                throw new LogicException(errorMessage);
            }
            return result;
        }
#endif
        /// <summary>
        /// Retrieves an object of type T by its ID asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="requestUri">The URI of the API endpoint.</param>
        /// <param name="id">The ID of the object to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the retrieved object of type T, or null if not found.</returns>
        /// <exception cref="LogicException">Thrown when the response from the API is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result;
        }

        /// <summary>
        /// Sends an asynchronous GET request to the specified URI and returns the deserialized content as an array of type T.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized content.</typeparam>
        /// <param name="requestUri">The URI of the resource to request.</param>
        /// <returns>An array of type T representing the deserialized content of the response.</returns>
        /// <exception cref="LogicException">Thrown if the response indicates an error.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Sends an asynchronous GET request to the specified URI and returns an array of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="requestUri">The URI to send the request to.</param>
        /// <param name="orderBy">The string parameter used to sort the request.</param>
        /// <returns>An array of type T that represents the response data, or an empty array if the response is not successful.</returns>
        /// <exception cref="LogicException">Thrown when the response is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Retrieves a paged list asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="size">The size of the page to retrieve.</param>
        /// <returns>An array of type T representing the paged list.</returns>
        /// <exception cref="LogicException">Thrown when the page retrieval fails.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Retrieves a paged list of items asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="orderBy">The field to order the list by.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="size">The number of items per page.</param>
        /// <returns>An array of items of type T.</returns>
        /// <exception cref="LogicException">Thrown when the response status code indicates an error.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }

        /// <summary>
        /// Queries a specified URI with a predicate asynchronously and returns an array of type T.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <param name="requestUri">The URI to query.</param>
        /// <param name="predicate">The predicate to use in the query.</param>
        /// <returns>An awaitable task that represents the asynchronous operation and holds the queried result as an array of type T.</returns>
        /// <exception cref="LogicException">Thrown when the response from the URI retrieval is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Asynchronously performs a query on the specified request URI with the given predicate and order by parameters.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="requestUri">The URI to send the query request.</param>
        /// <param name="predicate">The predicate to filter the query results.</param>
        /// <param name="orderBy">The order by criteria for sorting the query results.</param>
        /// <returns>An asynchronous task that represents the query operation. The task result contains an array of type T.</returns>
        /// <exception cref="LogicException">Thrown when the query failed.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Queries the specified request URI asynchronously and returns all results that match the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the result items.</typeparam>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="predicate">The predicate to filter the results.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="size">The size (number of items per page) of the page to retrieve.</param>
        /// <returns>An array of type T containing the queried results.</returns>
        /// <exception cref="LogicException">Thrown when the response is not successful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Queries all items asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="predicate">The predicate to filter the items.</param>
        /// <param name="orderBy">The ordering criteria.</param>
        /// <param name="index">The starting index of the page.</param>
        /// <param name="size">The size of the page.</param>
        /// <returns>An array of items.</returns>
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
                throw new LogicException(errorMessage);
            }
            return result ?? Array.Empty<T>();
        }

        /// <summary>
        /// Sends a POST request asynchronously to the specified URI and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type of the model to be serialized and deserialized.</typeparam>
        /// <param name="requestUri">The URI to send the POST request to.</param>
        /// <param name="model">The model to be serialized and sent as the request body.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deserialized response.</returns>
        /// <exception cref="LogicException">Thrown when the response is unsuccessful.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result!;
        }

        /// <summary>
        /// Sends a PUT request to the specified URI with the provided model and ID, and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="requestUri">The URI of the resource to update.</param>
        /// <param name="id">The ID of the resource to update.</param>
        /// <param name="model">The model to update the resource with.</param>
        /// <returns>The updated resource.</returns>
        /// <exception cref="LogicException">Thrown when the response is not a success status code.</exception>
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
                throw new LogicException(errorMessage);
            }
            return result!;
        }

        /// <summary>
        /// Asynchronously sends a DELETE request to the specified URI with the given ID.
        /// </summary>
        /// <param name="requestUri">The URI to which the DELETE request is sent.</param>
        /// <param name="id">The ID used in the request URL for deletion.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string requestUri, IdType id)
        {
            using var client = CreateClient();
            var response = await client.DeleteAsync($"{requestUri}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                var errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new LogicException(errorMessage);
            }
        }
    }
}
//MdEnd


