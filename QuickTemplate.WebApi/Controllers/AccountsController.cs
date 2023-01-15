//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Models.Account;
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountsController : ApiControllerBase
    {
        /// <summary>  
        /// This method checks the login data email/password and, if correct, returns a logon session.  
        /// </summary>  
        /// <param name="logonModel">The logon data.</param>  
        /// <returns>The logon session object.</returns>  
        [HttpPost("logon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Models.Account.LoginSession>> LogonByAsync(
            [FromBody] LogonModel logonModel)
        {
            var result = await Logic.AccountAccess.LogonAsync(logonModel.Email, logonModel.Password, logonModel.Info ?? string.Empty);

            return Ok(WebApi.Models.Account.LoginSession.Create(result));
        }

        /// <summary>  
        /// This query determines the payments depending on the parameters.  
        /// </summary>  
        /// <param name="sessionToken">The sessionToken.</param>  
        /// <returns>The logon session object.</returns>  
        [HttpPost("logout")]
        public Task LogoutByAsync([FromQuery(Name = "sessionToken")] string sessionToken)
        {
            return Logic.AccountAccess.LogoutAsync(sessionToken);
        }
    }
}
#endif
//MdEnd
