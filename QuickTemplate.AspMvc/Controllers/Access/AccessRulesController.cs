﻿//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.AspMvc.Controllers.Access
{
    using Microsoft.AspNetCore.Mvc;
    using TAccessModel = Logic.Models.Access.AccessRule;
    using TViewModel = AspMvc.Models.Access.AccessRule;
    using TFilterModel = AspMvc.Models.Access.AccessRuleFilter;
    using TAccessContract = Logic.Contracts.Access.IAccessRulesAccess;
    ///
    /// Generated by the generator
    ///
    public sealed partial class AccessRulesController : Controllers.FilterGenericController<TAccessModel, TViewModel, TFilterModel, TAccessContract>
    {
        ///
        /// Generated by the generator
        ///
        static AccessRulesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the constructor of the class is executed.
        /// </summary>
        /// <remarks>
        /// This method can be used to initialize any properties or variables before the class object is constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        /// <remarks>
        /// It is a partial method that can be implemented in a partial class to customize the behavior when the class is constructed.
        /// </remarks>
        static partial void ClassConstructed();
        /// <summary>
        /// Gets the name of the controller for access rules.
        /// </summary>
        /// <value>
        /// The name of the controller for access rules.
        /// </value>
        protected override string ControllerName => "AccessRules";
        ///
        /// Generated by the generator
        ///
        public AccessRulesController(TAccessContract other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// Invoked when the object is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is called during the construction process of the object.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// Represents a method that is called when the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is a partial method that can be implemented in a partial class or a partial struct.
        /// It is automatically called by the constructor of the class or struct in which it is implemented.
        /// </remarks>
        /// <returns>None</returns>
        partial void Constructed();
        ///
        /// Generated by the generator
        ///
        protected override TViewModel ToViewModel(TAccessModel accessModel, ActionMode actionMode)
        {
            var handled = false;
            var result = default(TViewModel);
            BeforeToViewModel(accessModel, actionMode, ref result, ref handled);
            if (handled == false || result == null)
            {
                result = TViewModel.Create(accessModel);
            }
            AfterToViewModel(result, actionMode);
            return BeforeView(result, actionMode);
        }
        /// <summary>
        /// Performs operations before converting the specified access model to its corresponding view model.
        /// </summary>
        /// <param name="accessModel">The access model to be converted.</param>
        /// <param name="actionMode">The mode of action to be performed.</param>
        /// <param name="viewModel">The reference to the view model being created. Can be null.</param>
        /// <param name="handled">Indicates whether the conversion has been handled by this method.</param>
        /// <remarks>
        /// This method is a hook for performing additional logic and customization before converting the access model
        /// to the view model. It is executed before the conversion takes place.
        /// </remarks>
        partial void BeforeToViewModel(TAccessModel accessModel, ActionMode actionMode, ref TViewModel? viewModel, ref bool handled);
        /// <summary>
        /// Executes code after converting a ViewModel.
        /// </summary>
        /// <param name="viewModel">The ViewModel to be converted.</param>
        /// <param name="actionMode">The action mode for this conversion.</param>
        /// <remarks>
        /// This method is invoked after converting a ViewModel and can be used to perform additional operations or modifications on the converted ViewModel.
        /// The <paramref name="actionMode"/> can be used to determine the specific action that triggered this conversion.
        /// </remarks>
        partial void AfterToViewModel(TViewModel viewModel, ActionMode actionMode);
    }
}
#endif
//MdEnd

