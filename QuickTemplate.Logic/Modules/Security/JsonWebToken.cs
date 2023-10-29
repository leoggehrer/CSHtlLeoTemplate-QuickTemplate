//@BaseCode
//MdStart
#if ACCOUNT_ON
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace QuickTemplate.Logic.Modules.Security
{
    /// <summary>
    /// Represents a JSON Web Token.
    /// </summary>
    internal partial class JsonWebToken
    {
        /// <summary>
        /// Initializes the JsonWebToken class and sets the necessary properties.
        /// </summary>
        static JsonWebToken()
        {
            ClassConstructing();
            var settingValue = Authorization.DefaultTimeOutInSeconds.ToString();
            
            Key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            Issuer = nameof(QuickTemplate);
            Audience = nameof(Logic);
            
            try
            {
                Key = Configuration.AppSettings.Get($"JwtSetting:{nameof(Key)}", Key);
                Issuer = Configuration.AppSettings.Get($"JwtSetting:{nameof(Issuer)}", Issuer);
                Audience = Configuration.AppSettings.Get($"JwtSetting:{nameof(Audience)}", Audience);
                settingValue = Configuration.AppSettings.Get($"JwtSetting:{nameof(TimeOutInSec)}", settingValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroor init Jwt: {ex.Message}");
            }
            TimeOutInSec = Convert.ToInt32(settingValue);
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when a class is being constructed.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any pre-construction logic, such as initializing variables or setting default values.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        internal static string Key { get; set; }
        /// <summary>
        /// Gets or sets the issuer of the property.
        /// </summary>
        /// <remarks>
        /// This property is used to store the issuer information.
        /// </remarks>
        internal static string Issuer { get; set; }
        /// <summary>
        /// Gets or sets the audience for the application.
        /// </summary>
        /// <value>
        /// The audience for the application.
        /// </value>
        internal static string Audience { get; set; }
        /// <summary>
        /// Represents the timeout period in seconds.
        /// </summary>
        /// <value>
        /// The duration (in seconds) before a timeout occurs.
        /// </value>
        /// <remarks>
        /// This property is used to specify the maximum amount of time allowed for
        /// an operation to complete before it is considered timed out.
        /// </remarks>
        internal static int TimeOutInSec { get; set; }
        
        /// <summary>
        /// Generates a JWT token based on the provided claims.
        /// </summary>
        /// <param name="claimsParam">The claims to include in the token.</param>
        /// <returns>The generated JWT token.</returns>
        internal static string GenerateToken(IEnumerable<Claim> claimsParam)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var secToken = new JwtSecurityToken(
            signingCredentials: credentials,
            issuer: Issuer,
            audience: Audience,
            claims: claimsParam,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(TimeOutInSec));
            
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }
        /// <summary>
        /// Checks if a token is valid.
        /// </summary>
        /// <param name="token">The token to be checked.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        public static bool CheckToken(string token)
        {
            return CheckToken(token, out _);
        }
        /// <summary>
        /// Checks if the given token is valid and sets the validatedToken parameter with the validated token.
        /// </summary>
        /// <param name="token">The token that needs to be checked.</param>
        /// <param name="validatedToken">The validated token, if the validation is successful; otherwise, null.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        internal static bool CheckToken(string token, out SecurityToken? validatedToken)
        {
            var result = false;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            
            validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                result = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
            }
            return result;
        }
        /// <summary>
        /// Retrieves the token validation parameters.
        /// </summary>
        /// <returns>The token validation parameters.</returns>
        /// <remarks>
        /// This method returns the token validation parameters used for validating tokens.
        /// It sets the following properties of the token validation parameters:
        /// - ValidateLifetime: False, because there is no expiration in the generated token.
        /// - ValidateAudience: False, because there is no audience in the generated token.
        /// - ValidateIssuer: False, because there is no issuer in the generated token.
        /// - ValidIssuer: The issuer value.
        /// - ValidAudience: The audience value.
        /// - IssuerSigningKey: A symmetric security key generated from the key string.
        /// </remarks>
        internal static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)) // The same key as the one that generate the token
            };
        }
    }
}
#endif
//MdEnd

