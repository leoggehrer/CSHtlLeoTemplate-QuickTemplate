//@BaseCode
//MdStart
#if ACCOUNT_ON
using Microsoft.AspNetCore.Mvc;
using QuickTemplate.AspMvc.Models.Account;

namespace QuickTemplate.AspMvc.Controllers.Account
{
    /// <summary>
    /// Represents a controller for managing user accounts.
    /// </summary>
    public partial class AccountController : MvcController
    {
        /// <summary>
        /// Represents the Account Controller class.
        /// </summary>
        static AccountController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Represents a method that is called before the constructing of a class.
        /// This method is declared as partial, allowing it to be implemented in different files.
        /// </summary>
        static partial void ClassConstructing();
        ///<summary>
        ///This method is called when a class is constructed.
        ///</summary>
        static partial void ClassConstructed();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        : base()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any initialization or setup tasks required during object construction.
        /// </remarks>
        /// <seealso cref="YourClass"/>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object has been constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be overridden in derived classes
        /// to perform any additional initialization tasks.
        /// </remarks>
        partial void Constructed();
        
        /// <summary>
        /// Logs in the user and returns the corresponding view for the logon process.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect the user to after successful logon.</param>
        /// <param name="error">An optional error message to display to the user.</param>
        /// <returns>The IActionResult representing the logon view.</returns>
        public IActionResult Logon(string? returnUrl = null, string? error = null)
        {
            var handled = false;
            var viewName = nameof(Logon);
            var viewModel = new LogonViewModel()
            {
                ReturnUrl = returnUrl,
            };
            
            BeforeLogon(viewModel, ref handled);
            if (handled == false)
            {
                SessionWrapper.ReturnUrl = returnUrl;
                if (error.HasContent())
                ViewBag.Error = error;
            }
            AfterLogon(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        /// <summary>
        /// Performs additional logic before the logon process.
        /// </summary>
        /// <param name="model">The LogonViewModel object containing logon information.</param>
        /// <param name="handled">A reference to a boolean value indicating if the logon process has been handled.</param>
        /// <remarks>
        /// This method is called before the logon process and allows for additional logic to be performed.
        /// Any changes made to the LogonViewModel object will affect the logon process.
        /// The handled parameter can be set to true to indicate that the logon process has been handled and no further processing is required.
        /// </remarks>
        partial void BeforeLogon(LogonViewModel model, ref bool handled);
        /// <summary>
        /// This method is called after a user logs on.
        /// </summary>
        /// <param name="model">The LogonViewModel object representing the logged on user's information.</param>
        /// <param name="viewName">The name of the view that should be displayed after logging on.</param>
        /// <remarks>
        /// The method is marked as partial, which means that its implementation can be split across multiple files or parts within a single class.
        /// </remarks>
        /// <seealso cref="LogonViewModel"/>
        /// <remarks>
        /// Any modifications to the viewName parameter will be reflected in the calling code since it is passed by reference.
        /// </remarks>
        partial void AfterLogon(LogonViewModel model, ref string viewName);
        
        /// <summary>
        /// Redirects to the Index action of the Home Controller.
        /// </summary>
        /// <returns>Returns an IActionResult object representing the result of the action.</returns>
        [HttpGet]
        [ActionName("Index")]
        public virtual IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        
        /// <summary>
        /// Logon method that handles the HTTP POST request for user login.
        /// </summary>
        /// <param name="viewModel">The LogonViewModel object containing the user login information.</param>
        /// <returns>An asynchronous task result representing the action result to be performed.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Logon")]
        public async Task<IActionResult> LogonAsync(LogonViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var action = SessionWrapper.ReturnAction ?? "Index";
            var controller = SessionWrapper.ReturnController ?? "Home";
            
            BeforeDoLogon(viewModel, ref handled);
            if (handled == false)
            {
                try
                {
                    await ExecuteLogonAsync(viewModel).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.GetError();
                    return View(viewModel);
                }
            }
            AfterDoLogon(viewModel, ref action, ref controller);
            if (viewModel.ReturnUrl != null)
            {
                return Redirect(viewModel.ReturnUrl);
            }
            return RedirectToAction(action, controller);
        }
        /// <summary>
        /// Handles the pre-logon event before logging in.
        /// </summary>
        /// <param name="model">The <see cref="LogonViewModel"/> object containing the logon information.</param>
        /// <param name="handled">A reference to a boolean value indicating whether the event has been handled.</param>
        /// <remarks>
        /// This method is called before a user logs in. It allows developers to perform pre-processing tasks
        /// or manipulate the logon view model before the logon process begins.
        /// </remarks>
        /// <seealso cref="LogonViewModel"/>
        /// <seealso cref="AfterDoLogon(LogonViewModel)"/>
        partial void BeforeDoLogon(LogonViewModel model, ref bool handled);
        /// <summary>
        /// Executes a set of actions after a user logs in.
        /// </summary>
        /// <param name="model">The LogonViewModel representing the user's logon information.</param>
        /// <param name="action">A string variable to be modified to set the action to be performed after logon.</param>
        /// <param name="controller">A string variable to be modified to set the controller to be used after logon.</param>
        partial void AfterDoLogon(LogonViewModel model, ref string action, ref string controller);
        
        /// <summary>
        /// Handles remote logon requests and returns the appropriate view with the logon view model.
        /// </summary>
        /// <param name="returnUrl">Optional. The URL to redirect to after successful logon.</param>
        /// <param name="error">Optional. Error message to display in case of logon failure.</param>
        /// <returns>
        /// IActionResult representing the appropriate view with the logon view model.
        /// </returns>
        public IActionResult LogonRemote(string? returnUrl = null, string? error = null)
        {
            var handled = false;
            var viewName = nameof(LogonRemote);
            var viewModel = new LogonViewModel()
            {
                ReturnUrl = returnUrl,
            };
            
            BeforeLogonRemote(viewModel, ref handled);
            if (handled == false)
            {
                SessionWrapper.ReturnUrl = returnUrl;
                if (error.HasContent())
                ViewBag.Error = error;
            }
            AfterLogonRemote(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        /// <summary>
        /// This method is called before the remote logon is performed.
        /// </summary>
        /// <param name="model">The logon view model containing the logon information.</param>
        /// <param name="handled">A boolean flag indicating whether the event has been handled.</param>
        /// <remarks>
        /// This method is called during the logon process, just before the remote logon is performed.
        /// It allows for custom processing or validation to be implemented before the logon operation.
        /// </remarks>
        partial void BeforeLogonRemote(LogonViewModel model, ref bool handled);
        /// <summary>
        /// This method is called after a remote logon operation.
        /// </summary>
        /// <param name="model">The logon view model.</param>
        /// <param name="viewName">The name of the view to display.</param>
        /// <remarks>
        /// This method can be used to perform any additional logic or actions
        /// after a successful logon operation. It allows modifying the view name
        /// to navigate to a different view if needed.
        /// </remarks>
        /// <seealso cref="LogonViewModel"/>
        partial void AfterLogonRemote(LogonViewModel model, ref string viewName);
        
        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the logout operation.</returns>
        [ActionName("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (SessionWrapper.LoginSession != null)
            {
                bool handled = false;
                
                BeforeLogout(ref handled);
                if (handled == false)
                {
                    await Logic.AccountAccess.LogoutAsync(SessionWrapper.LoginSession.SessionToken).ConfigureAwait(false);
                    SessionWrapper.LoginSession = null;
                }
                AfterLogout();
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Executes before the logout process.
        /// </summary>
        /// <param name="handled">Indicates whether the logout event has been handled. By default, it's set to false.</param>
        /// <remarks>
        /// The BeforeLogout method is a partial method that allows custom logic to be executed before the logout process
        /// takes place. This method is intended to be implemented by the consuming code, and it can be used to perform any
        /// necessary pre-logout actions or validations. The 'handled' parameter is provided to allow the consuming code
        /// to indicate whether it has handled the logout process. If 'handled' is set to true, the logout process will
        /// get skipped.
        /// </remarks>
        partial void BeforeLogout(ref bool handled);
        /// <summary>
        /// Executed after a user logs out from the system.
        /// </summary>
        /// <remarks>
        /// This method can be overridden by classes that implement the current interface.
        /// Use this method to perform any necessary cleanup actions or trigger additional functionality
        /// after the user has successfully logged out.
        /// </remarks>
        partial void AfterLogout();
        
        /// <summary>
        /// Returns the view for changing the password.
        /// </summary>
        /// <returns>The action result representing the view for changing the password.</returns>
        public IActionResult ChangePassword()
        {
            var handled = false;
            var viewModel = new ChangePasswordViewModel();
            var viewName = "ChangePassword";
            
            BeforeChangePassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }
                viewModel.UserName = SessionWrapper.LoginSession?.Name ?? string.Empty;
                viewModel.Email = SessionWrapper.LoginSession?.Email ?? String.Empty;
            }
            AfterChangePassword(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        /// <summary>
        /// Method called before changing the password.
        /// </summary>
        /// <param name="model">The view model containing the password information.</param>
        /// <param name="handled">A boolean reference indicating whether the method handled the password change</param>
        partial void BeforeChangePassword(ChangePasswordViewModel model, ref bool handled);
        ///<summary>
        /// This method is called after a user has successfully changed their password.
        /// It allows for additional processing or customization to be done after the password change.
        ///</summary>
        ///<param name="model">The ChangePasswordViewModel object representing the updated password model.</param>
        ///<param name="viewName">The name of the view to be rendered after the password change.</param>
        partial void AfterChangePassword(ChangePasswordViewModel model, ref string viewName);
        
        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="viewModel">The view model containing the old and new password</param>
        /// <returns>An asynchronous task that returns an IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var viewName = "ConfirmationChangePassword";
            
            BeforeDoChangePassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }
                
                try
                {
                    await Logic.AccountAccess.ChangePasswordAsync(SessionWrapper.LoginSession?.SessionToken ?? string.Empty, viewModel.OldPassword, viewModel.NewPassword).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.GetError();
                    return View("ChangePassword", viewModel);
                }
            }
            AfterDoChangePassword(viewModel, ref viewName);
            return View(viewName);
        }
        /// <summary>
        /// This method is called before the "DoChangePassword" operation is performed.
        /// </summary>
        /// <param name="model">The <see cref="ChangePasswordViewModel"/> object containing the new password details.</param>
        /// <param name="handled">A reference to a boolean value indicating if the operation has been handled.</param>
        /// <remarks>
        ///   This method allows for performing additional operations or validations before
        ///   the "DoChangePassword" operation is executed. It is called by the main operation,
        ///   allowing for customizations or overriding in partial classes.
        /// </remarks>
        partial void BeforeDoChangePassword(ChangePasswordViewModel model, ref bool handled);
        /// <summary>
        /// This method is called after the "DoChangePassword" action is performed in the application.
        /// It allows additional logic to be executed after the change password operation.
        /// </summary>
        /// <param name="model">The ChangePasswordViewModel object containing the data from the change password form.</param>
        /// <param name="viewName">The name of the view to be rendered after the change password operation.</param>
        partial void AfterDoChangePassword(ChangePasswordViewModel model, ref string viewName);
        
        /// <summary>
        /// This method is responsible for resetting the password.
        /// </summary>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        public IActionResult ResetPassword()
        {
            var handled = false;
            var viewModel = new ResetPasswordViewModel();
            var viewName = "ResetPassword";
            
            BeforeResetPassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }
            }
            AfterResetPassword(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        /// <summary>
        /// Performs actions before resetting a user's password.
        /// </summary>
        /// <param name="model">The view model containing the information needed for resetting the password.</param>
        /// <param name="handled">A reference to a boolean value indicating whether the reset password operation has been handled.</param>
        /// <remarks>
        /// This method is called before the actual password reset operation takes place. It allows for additional custom logic
        /// or validation to be performed before the password is reset.
        /// </remarks>
        partial void BeforeResetPassword(ResetPasswordViewModel model, ref bool handled);
        /// <summary>
        /// This method is executed after the reset password action and allows for additional processing.
        /// </summary>
        /// <param name="model">The <see cref="ResetPasswordViewModel"/> object containing the data for the reset password action.</param>
        /// <param name="viewName">A reference to the string that contains the name of the view to be rendered.</param>
        partial void AfterResetPassword(ResetPasswordViewModel model, ref string viewName);
        
        /// <summary>
        /// Resets the password for a user asynchronously.
        /// </summary>
        /// <param name="viewModel">The view model containing the necessary information for password reset.</param>
        /// <returns>An IActionResult representing the result of the password reset operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var viewName = "ConfirmationResetPassword";
            
            BeforeDoResetPassword(viewModel, ref handled);
            if (SessionWrapper.LoginSession == null
            || SessionWrapper.LoginSession.LogoutTime.HasValue)
            {
                return RedirectToAction("Logon", new { returnUrl = "ResetPassword" });
            }
            
            try
            {
                await Logic.AccountAccess.ChangePasswordForAsync(SessionWrapper.SessionToken, viewModel.Email, viewModel.ConfirmPassword).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetError();
                return View("ResetPassword", viewModel);
            }
            AfterDoResetPassword(viewModel, ref viewName);
            return View(viewName);
        }
        /// <summary>
        /// This method is called before executing the ResetPassword method.
        /// </summary>
        /// <param name="model">The ResetPasswordViewModel object containing the data for resetting the password.</param>
        /// <param name="handled">A flag indicating whether the method has been handled. If set to true, the method is considered as handled and will not be executed further.</param>
        partial void BeforeDoResetPassword(ResetPasswordViewModel model, ref bool handled);
        /// <summary>
        /// This method is triggered after the password reset is performed.
        /// </summary>
        /// <param name="model">The reset password view model containing the updated password information.</param>
        /// <param name="viewName">A string reference to the name of the view to be displayed after the password reset.</param>
        /// <remarks>
        /// This method can be implemented in a partial class to handle additional logic after resetting the password.
        /// It allows customization of the view name to be displayed, providing flexibility in the navigation flow.
        /// </remarks>
        /// <seealso cref="ResetPasswordViewModel"/>
        /// <exception cref="ArgumentNullException">Thrown when the 'model' argument is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the 'viewName' argument is null or empty.</exception>
        partial void AfterDoResetPassword(ResetPasswordViewModel model, ref string viewName);
        
        /// <summary>
        /// Executes the logon process asynchronously.
        /// </summary>
        /// <param name="viewModel">The view model containing the email and password for logon.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task ExecuteLogonAsync(LogonViewModel viewModel)
        {
            try
            {
                var loginSession = await Logic.AccountAccess.LogonAsync(viewModel.Email, viewModel.Password).ConfigureAwait(false);
                
                if (loginSession != null)
                {
                    SessionWrapper.LoginSession = Models.Account.LoginSession.Create(loginSession);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetError();
                throw;
            }
        }
    }
}
#endif
//MdEnd


