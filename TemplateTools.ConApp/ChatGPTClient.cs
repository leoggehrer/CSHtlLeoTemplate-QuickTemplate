//@CodeCopy
//MdStart
using RestSharp;
using System.Reflection;
using System.Text.Json;
using TemplateTooles.ConApp.Models.ChatGPT;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents a client for interacting with the ChatGPT API.
    /// </summary>
    internal partial class ChatGPTClient
    {
        /// <summary>
        /// Initializes the <see cref="ChatGPTClient"/> class.
        /// </summary>
        static ChatGPTClient()
        {
            BeforeClassInitialize();
            try
            {
                var configuration = CommonBase.Modules.Configuration.Configurator.LoadAppSettings();
                
                _baseUrl = configuration["ChatGPT:Url"] ??= string.Empty;
                _secretApIKey = configuration["ChatGPT:ApiKey"] ??= string.Empty;
            }
            catch (Exception ex)
            {
                _baseUrl = string.Empty;
                _secretApIKey = string.Empty;
                System.Diagnostics.Debug.WriteLine(message: $"Error in {System.Reflection.MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
            }
            AfterClassInitialize();
        }
        /// <summary>
        /// This method is called before the initialization of the test class.
        /// </summary>
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the initialization of the class.
        /// </summary>
        /// <remarks>
        /// Add any additional details or remarks about the method here.
        /// </remarks>
        static partial void AfterClassInitialize();
        
        private static readonly string _baseUrl;
        private static readonly string _secretApIKey;
        /// <summary>
        /// Gets or sets the serializer options for deserializing JSON.
        /// </summary>
        /// <value>
        /// The serializer options for deserializing JSON.
        /// </value>
        private static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        
        private readonly string _url;
        private readonly string _apiKey;
        private readonly RestClient _client;
        
        /// <summary>
        /// Initializes a new instance of the ChatGPTClient class.
        /// </summary>
        /// <remarks>
        /// This constructor sets the default values for the API url and API key,
        /// and creates a RestClient object with the specified base URL.
        /// </remarks>
        public ChatGPTClient()
        {
            _url = _baseUrl;
            _apiKey = _secretApIKey;
            _client = new RestClient(_url);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGPTClient"/> class.
        /// </summary>
        /// <param name="url">The URL of the ChatGPT server.</param>
        /// <param name="apiKey">The API key for accessing the ChatGPT server.</param>
        public ChatGPTClient(string url, string apiKey)
        {
            _url = url;
            _apiKey = apiKey;
            _client = new RestClient(_url);
        }
        
        /// <summary>
        /// Sends a message synchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A response object indicating the result of the operation.</returns>
        public Response SendMessage(string message)
        {
            return Task.Run(async() => await SendMessageAsync(message)).Result;
        }
        /// <summary>
        /// Sends a message asynchronously and receives a response from the API.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>A task representing the asynchronous operation. The response from the API.</returns>
        /// <exception cref="ArgumentException">Thrown when the message is null, empty, or contains only whitespace.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the API request.</exception>
        public async Task<Response> SendMessageAsync(string message)
        {
            // Check for empty input
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(null, nameof(message));
            }
            
            try
            {
                // Create a new POST request
                var request = new RestRequest("", Method.Post);
                // Set the Content-Type header
                request.AddHeader("Content-Type", "application/json");
                // Set the Authorization header with the API key
                request.AddHeader("Authorization", $"Bearer {_apiKey}");
                
                // Create the request body with the message and other parameters
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] {
                        new { role = "user", content = message },
                    },
                    temperature = 1.0,
                };
                
                // Add the JSON body to the request
                request.AddJsonBody(JsonSerializer.Serialize(requestBody));
                
                // Execute the request and receive the response
                var response = await _client.ExecuteAsync(request).ConfigureAwait(false);
                
                // Deserialize the response JSON content
                var result = JsonSerializer.Deserialize<Response>(response.Content ?? string.Empty, DeserializerOptions);
                
                // Extract and return the chatbot's response text
                return result ?? new Response();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API request
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                
                throw;
            }
        }
    }
}
//MdEnd
