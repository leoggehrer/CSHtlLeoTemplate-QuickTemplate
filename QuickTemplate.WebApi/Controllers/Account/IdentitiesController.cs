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
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        ///
        /// Implemented from template developer
        ///
        public IdentitiesController(QuickTemplate.Logic.Contracts.Account.IIdentitiesAccess<TAccessModel> other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
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
        partial void BeforeToOutModel(TAccessModel accessModel, ref TOutModel? outModel, ref bool handled);
        partial void AfterToOutModel(TOutModel outModel);
    }
}
#endif
//MdEnd
