//@BaseCode
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
        /// <summary>
        /// Returns the media type for JSON formatting.
        /// </summary>
        /// <returns>The media type string.</returns>
        protected static string MediaType => "application/json";
        /// <summary>
        /// Gets the options for deserializing JSON data into objects.
        /// </summary>
        /// <value>
        /// The options for deserializing JSON data into objects.
        /// </value>
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        #endregion static properties
        
        #region static methods
        /// <summary>
        /// Creates a new instance of HttpClient with the specified base address.
        /// </summary>
        /// <param name="baseAddress">The base address of the server.</param>
        /// <returns>A new instance of HttpClient.</returns>
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
        /// Creates a new instance of HttpClient with the specified base address and session token.
        /// </summary>
        /// <param name="baseAddress">The base address of the HTTP client.</param>
        /// <param name="sessionToken">The session token to be used for authorization.</param>
        /// <returns>A new instance of HttpClient.</returns>
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
        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        public string BaseAddress { get; init; }
        /// <summary>
        /// Gets or sets the session token for the current session.
        /// </summary>
        public string SessionToken { get; init; }
        #endregion properties
        
        #region constructors
        /// <summary>
        /// Initializes a new instance of the ClientAccess class with the specified base address and an empty token.
        /// </summary>
        /// <param name="baseAddress">The base address of the client.</param>
        public ClientAccess(string baseAddress)
        : this(baseAddress, string.Empty)
        {
        }
        /// <summary>
        /// Creates a new instance of the ClientAccess class.
        /// </summary>
        /// <param name="baseAddress">The base address for the client.</param>
        /// <param name="sessionToken">The session token for the client.</param>
        public ClientAccess(string baseAddress, string sessionToken)
        {
            BaseAddress = baseAddress;
            SessionToken = sessionToken;
        }
        #endregion constructors
        
        #region methods
        /// <summary>
        /// Creates an instance of <see cref="HttpClient"/> with the specified <see cref="BaseAddress"/> and <see cref="SessionToken"/> if available.
        /// </summary>
        /// <returns>An instance of <see cref="HttpClient"/>.</returns>
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
        /// Asynchronously retrieves the maximum page size from the specified request URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the request to.</param>
        /// <returns>The maximum page size as an integer.</returns>
        /// <exception cref="RestApiException">Thrown when the API request fails.</exception>
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
        
        /// <summary>
        /// Retrieves the count asynchronously from the specified request URI.
        /// </summary>
        /// <param name="requestUri">The URI of the request.</param>
        /// <returns>The count as an integer.</returns>
        /// <exception cref="RestApiException">Thrown when the response is not successful.</exception>
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
        /// <summary>
        /// Asynchronously retrieves the count of items from the specified request URI using the specified predicate.
        /// </summary>
        /// <param name="requestUri">The request URI to retrieve the count from.</param>
        /// <param name="predicate">The predicate to be used to filter the items.</param>
        /// <returns>The count of items as an integer.</returns>
        /// <exception cref="RestApiException">Thrown if the response is not a success status code.</exception>
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
        /// <summary>
        /// Retrieves an object of type T by its unique identifier asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="requestUri">The base URI of the request.</param>
        /// <param name="id">The unique identifier of the object.</param>
        /// <returns>An asynchronous task that represents the retrieval operation. The task result represents the retrieved object of type T if successful; otherwise, null.</returns>
        /// <exception cref="RestApiException">Thrown when the response status code is not successful. The exception message contains the response reason phrase along with the string data received from the response.</exception>
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
        /// <summary>
        /// Retrieves an entity by its ID asynchronously from the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the entity to be retrieved.</typeparam>
        /// <param name="=requestUri">The relative URI of the endpoint.</param>
        /// <param name="id">The ID of the entity to be retrieved.</param> 
        /// <returns>
        /// A task representing the asynchronous retrieval operation, returning an object of type T.
        ///          If successful, the returned value is the deserialized entity.
        ///          If unsuccessful, an exception of type RestApiException is thrown.
        /// </returns>
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
        
        /// <summary>
        /// Retrieves data asynchronously from the specified URI and returns an array of type T.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="requestUri">The URI to make the GET request.</param>
        /// <returns>An array of type T representing the retrieved data.</returns>
        /// <exception cref="RestApiException">Thrown when the response is not a success status code.</exception>
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
        /// <summary>
        /// Sends an asynchronous GET request to the specified URI with sorting and returns the deserialized content as an array of type T.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="requestUri">The URI to send the request to.</param>
        /// <param name="orderBy">The sorting criteria.</param>
        /// <returns>An array of type T representing the deserialized content.</returns>
        /// <exception cref="RestApiException">Thrown when the GET request fails or returns an error status code.</exception>
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
        /// <summary>
        /// Asynchronously gets a paginated list of items of type T from the specified request URI.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="requestUri">The request URI to retrieve the list from.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="size">The size of each page.</param>
        /// <returns>An array of items of type T.</returns>
        /// <exception cref="RestApiException">Thrown when the HTTP response is unsuccessful.</exception>
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
        /// <summary>
        /// Gets a paged list of items asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="requestUri">The URI of the resource.</param>
        /// <param name="orderBy">The field to order the list by.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="size">The size of the page.</param>
        /// <returns>An array of items.</returns>
        /// <exception cref="RestApiException">Thrown when the API request fails.</exception>
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
        
        /// <summary>
        /// Sends an asynchronous HTTP GET request to the specified URI and queries the result with the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the payload in the response.</typeparam>
        /// <param name="requestUri">The URI to send the GET request to.</param>
        /// <param name="predicate">The predicate used to query the result.</param>
        /// <returns>An array of type T containing the deserialized response data.</returns>
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
        /// <summary>
        /// Queries all resources asynchronously based on the provided request URI, predicate, and order by parameters.
        /// </summary>
        /// <typeparam name="T">Type of the resources returned by the query.</typeparam>
        /// <param name="requestUri">The URI to send the query request to.</param>
        /// <param name="predicate">The predicate to specify the condition for the query.</param>
        /// <param name="orderBy">The property to order the query results by.</param>
        /// <returns>An array of resources of type T that match the criteria.</returns>
        /// <exception cref="RestApiException">Thrown when the query fails due to an unsuccessful HTTP response.</exception>
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
        /// <summary>
        /// Queries all records asynchronously based on the specified request URL, predicate, index, and size.
        /// </summary>
        /// <typeparam name="T">The type of the query result.</typeparam>
        /// <param name="requestUri">The URL of the endpoint for querying.</param>
        /// <param name="predicate">The predicate used for querying.</param>
        /// <param name="index">The index of the queried page.</param>
        /// <param name="size">The size of the queried page.</param>
        /// <returns>An array of <typeparamref name="T"/> representing the query result.</returns>
        /// <exception cref="RestApiException">Thrown when the API response indicates an error.</exception>
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
        /// <summary>
        /// Queries all items asynchronously based on the provided parameters.
        /// </summary>
        /// <typeparam name="T">The type of objects to query.</typeparam>
        /// <param name="requestUri">The base URI of the request.</param>
        /// <param name="predicate">The predicate to filter the items.</param>
        /// <param name="orderBy">The property to order the items.</param>
        /// <param name="index">The index of the requested page.</param>
        /// <param name="size">The size of the requested page.</param>
        /// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
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
        
        /// <summary>
        /// Sends a POST request to the specified URI with the given model as the request body and returns the deserialized response of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model and the response.</typeparam>
        /// <param name="requestUri">The URI to which the request is sent.</param>
        /// <param name="model">The model to be sent as the request body.</param>
        /// <returns>The deserialized response of type <typeparamref name="T"/>.</returns>
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
        
        /// <summary>
        /// Sends a PUT request to the specified URI with the provided model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="id">The ID of the model.</param>
        /// <param name="model">The model to be sent in the request body.</param>
        /// <returns>The deserialized response model.</returns>
        /// <exception cref="RestApiException">Thrown when the response status is not successful.</exception>
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

        /// <summary>
        /// Deletes a resource asynchronously from the specified request URI using the specified ID.
        /// </summary>
        /// <param name="requestUri">The request URI to send the delete request.</param> 
        /// <param name="id">The ID of the resource to delete.</param> 
        /// <exception cref="RestApiException">If the delete operation fails.</exception> 
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



