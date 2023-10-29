//@BaseCode
//MdStart
using Microsoft.AspNetCore.Mvc;
using QuickTemplate.AspMvc.Modules.Session;
#if ACCOUNT_ON
using Microsoft.AspNetCore.Mvc.Filters;
using QuickTemplate.AspMvc.Controllers.Account;
#endif
namespace QuickTemplate.AspMvc.Controllers
{
    public partial class MvcController : Controller
    {
        /// <summary>
        /// Initializes static members of the MvcController class.
        /// </summary>
        /// <remarks>
        /// This method is called automatically when the MvcController class is first accessed.
        /// </remarks>
        static MvcController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called just before the constructor of the class is called, allowing any custom initialization logic to be performed.
        /// </summary>
        /// <remarks>
        /// This method is implemented in a partial class and can be used to add additional logic to the constructor or to initialize any class-specific variables.
        /// </remarks>
        /// <seealso cref="ClassName"/>
        /// <seealso cref="ClassConstructingEvent"/>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is a partial method and should be implemented in a partial class
        /// to provide custom logic upon class construction.
        /// </remarks>
        static partial void ClassConstructed();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcController"/> class.
        /// </summary>
        public MvcController()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// Use this method to perform additional initialization tasks or set default values
        /// before the object is fully constructed.
        /// </remarks>
        /// <seealso cref="YourClassName.Constructing"/>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is fully constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be overridden in a partial class and can be used to perform any additional initialization logic
        /// after the object construction has completed.
        /// </remarks>
        /// <seealso cref="YourNamespace.YourClassName"/>
        partial void Constructed();
        
        #region SessionInfo
        /// <summary>
        /// Gets a value indicating whether the session is available for the current HTTP context.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the session is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsSessionAvailable => HttpContext?.Session != null;
        private ISessionWrapper? sessionWrapper = null;
        /// <summary>
        /// Gets or sets the internal session wrapper.
        /// </summary>
        /// <value>
        /// The internal session wrapper.
        /// </value>
        internal ISessionWrapper SessionWrapper => sessionWrapper ??= new SessionWrapper(HttpContext.Session);
        #endregion SessionInfo
        
#if ACCOUNT_ON
        /// <summary>
        /// Gets or sets a value indicating whether the session token should be checked.
        /// </summary>
        protected virtual bool CheckSessionToken { get; set; } = true;
        /// <summary>
        /// This method is called before the action method is executed.
        /// It checks the session token and ensures that the user is logged in and the session is still alive.
        /// If the session token is not valid, it redirects to the login page.
        /// </summary>
        /// <param name="context">The context for the action method being executed.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (CheckSessionToken && context.Controller?.GetType().Name.Equals(nameof(AccountController)) == false)
            {
                if (SessionWrapper.IsAuthenticated == false)
                {
                    ViewBag.Error = ("You are not yet registered. Please log in to the system.");
                    context.Result = new RedirectToActionResult("Logon", "Account", null);
                }
                else if (SessionWrapper.IsSessionAlive == false)
                {
                    ViewBag.Error = ("Your session has expired. Please sign in again.");
                    context.Result = new RedirectToActionResult("Logon", "Account", null);
                }
            }
            base.OnActionExecuting(context);
        }
#endif
        
        #region Error-helpers
        /// <summary>
        /// Retrieves the error messages from the model state and concatenates them into a single string.
        /// </summary>
        /// <returns>A string containing all the error messages from the model state.</returns>
        protected string GetModelStateError()
        {
            var errors = GetModelStateErrors();
            
            return string.Join($"{Environment.NewLine}", errors);
        }
        /// <summary>
        /// Retrieves a string array containing all the model state errors.
        /// </summary>
        /// <returns>
        /// A string array that contains the model state errors in the format "{property}: {error message}".
        /// </returns>
        protected string[] GetModelStateErrors()
        {
            var list = new List<string>();
            var errorLists = ModelState.Where(x => x.Value?.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value?.Errors });
            
            foreach (var errorList in errorLists)
            {
                if (errorList.Errors != null)
                {
                    foreach (var error in errorList.Errors)
                    {
                        list.Add($"{errorList.Key}: {error.ErrorMessage}");
                    }
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// Retrieves the error message from the specified Exception object.
        /// </summary>
        /// <param name="source">The Exception object from which to retrieve the error message.</param>
        /// <returns>The error message associated with the specified Exception object.</returns>
        protected static string GetExceptionError(Exception source)
        {
            return source.GetError();
        }
        #endregion Error-Helpers
    }
}
//MdEnd


