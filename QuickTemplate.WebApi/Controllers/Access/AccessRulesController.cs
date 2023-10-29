//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.WebApi.Controllers.Access
{
    ///
    /// Implemented from template developer
    ///
    public sealed partial class AccessRulesController : GenericController<Logic.Models.Access.AccessRule, WebApi.Models.Access.AccessRuleEdit, WebApi.Models.Access.AccessRule>
    {
        /// <summary>
        /// Represents a static class used for managing access rules.
        /// </summary>
        static AccessRulesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Represents the method called when a class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is called before the constructor of the class is executed.
        /// It can be implemented in a partial class to perform additional initialization
        /// or set up tasks before object creation.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        ///
        /// Implemented from template developer
        ///
        public AccessRulesController(Logic.Contracts.Access.IAccessRulesAccess other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object. It is intended to be implemented in partial classes to add additional logic and setup when the object is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is marked as partial so that it can be implemented in separate files for a single class. Each partial implementation will be called in the order of their declaration.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// Represents a method that is called when an object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is called during the construction of an object, typically after all the fields and properties
        /// have been initialized. The execution of this method is dependent on the implementation of the class where
        /// it is defined, and can be overridden by derived classes.
        /// </remarks>
        partial void Constructed();
        ///
        /// Implemented from template developer
        ///
        protected override WebApi.Models.Access.AccessRule ToOutModel(Logic.Models.Access.AccessRule accessModel)
        {
            var handled = false;
            var result = default(QuickTemplate.WebApi.Models.Access.AccessRule);
            BeforeToOutModel(accessModel, ref result, ref handled);
            if (handled == false || result == null)
            {
                result = WebApi.Models.Access.AccessRule.Create(accessModel);
            }
            AfterToOutModel(result);
            return result;
        }
        ///<summary>
        ///This method is called before converting an AccessRule from the logic layer to the web API layer.
        ///</summary>
        ///<param name="accessModel">The AccessRule object from the logic layer.</param>
        ///<param name="outModel">The AccessRule object from the web API layer (nullable).</param>
        ///<param name="handled">A boolean indicating if the BeforeToOutModel has been handled.</param>
        ///<remarks>
        ///This method allows for any pre-processing or modifications to be made to the AccessRule object
        ///before it is converted to the web API layer. By modifying the outModel parameter, the changes will
        ///be reflected in the web API layer. The handled parameter can be used to indicate if any modifications
        ///or processing has been done in this method.
        ///</remarks>
        partial void BeforeToOutModel(Logic.Models.Access.AccessRule accessModel, ref WebApi.Models.Access.AccessRule? outModel, ref bool handled);
        /// <summary>
        /// Represents a method that is called after converting a model to an output model in the API.
        /// </summary>
        /// <param name="outModel">The output model obtained after conversion.</param>
        /// <remarks>
        /// This method can be overridden in derived classes to perform additional operations
        /// or modifications on the output model after it has been generated from the input model.
        /// </remarks>
        /// <seealso cref="WebApi.Models.Access.AccessRule"/>
        partial void AfterToOutModel(WebApi.Models.Access.AccessRule outModel);
    }
}
#endif
//MdEnd

