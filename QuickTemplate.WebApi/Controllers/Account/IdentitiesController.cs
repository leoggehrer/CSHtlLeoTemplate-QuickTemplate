//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.WebApi.Controllers.Account
{
    using TAccessModel = Logic.Models.Account.Identity;
    using TOutModel = WebApi.Models.Account.Identity;
    using TEditModel = WebApi.Models.Account.IdentityEdit;
    ///
    /// Implemented from template developer
    ///
    public sealed partial class IdentitiesController : Controllers.GenericController<TAccessModel, TEditModel, TOutModel>
    {
        ///
        /// Implemented from template developer
        ///
        static IdentitiesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before a class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented by partial classes to perform any necessary operations
        /// before the construction of a class instance.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        ///
        /// Implemented from template developer
        ///
        public IdentitiesController(QuickTemplate.Logic.Contracts.Account.IIdentitiesAccess other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// Represents a placeholder method that can be overridden in a partial class.
        /// </summary>
        /// <remarks>
        /// This method can be used in a partial class to perform custom initialization
        /// logic when the containing object is being constructed.
        /// </remarks>
        /// <seealso cref="YourClassName"/>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object has been fully constructed and is ready for use.
        /// </summary>
        ///
        /// <remarks>
        /// This method is intended to be overridden in derived classes to perform additional initialization or setup tasks
        /// after the object has been constructed.
        /// </remarks>
        partial void Constructed();
        ///
        /// Implemented from template developer
        ///
        protected override TOutModel ToOutModel(TAccessModel accessModel)
        {
            var handled = false;
            var result = default(TOutModel);
            BeforeToOutModel(accessModel, ref result, ref handled);
            if (handled == false || result == null)
            {
                result = TOutModel.Create(accessModel);
            }
            AfterToOutModel(result);
            return result;
        }
        /// <summary>
        /// Performs actions before converting the input access model to the output model.
        /// </summary>
        /// <param name="accessModel">The input access model.</param>
        /// <param name="outModel">The output model, which is passed by reference.</param>
        /// <param name="handled">A boolean value indicating whether the actions have been handled.</param>
        partial void BeforeToOutModel(TAccessModel accessModel, ref TOutModel? outModel, ref bool handled);
        /// <summary>
        /// Represents the method that is called after converting a model to an output model.
        /// </summary>
        /// <param name="outModel">The output model that is generated.</param>
        partial void AfterToOutModel(TOutModel outModel);
    }
}
#endif
//MdEnd

