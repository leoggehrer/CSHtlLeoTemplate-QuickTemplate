﻿//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.WebApi.Controllers.Account
{
    using TAccessModel = Logic.Models.Account.Role;
    using TEditModel = WebApi.Models.Account.IdentityRoleEdit;
    using TOutModel = WebApi.Models.Account.IdentityRole;
    ///
    /// Generated by the generator
    ///
    public sealed partial class IdentityRolesController : Controllers.GenericController<TAccessModel, TEditModel, TOutModel>
    {
        ///
        /// Generated by the generator
        ///
        static IdentityRolesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        ///
        /// Generated by the generator
        ///
        public IdentityRolesController(Logic.Contracts.Account.IRolesAccess<TAccessModel> other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        ///
        /// Generated by the generator
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
