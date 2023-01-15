﻿//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.WebApi.Models.Account
{
    /// <summary>
    /// This model represents an account role.
    /// </summary>
    public partial class IdentityRoleEdit
    {
        /// <summary>
        /// Gets and sets a role designation.
        /// </summary>
        public string Designation { get; set; } = string.Empty;
        /// <summary>
        /// Gets and sets a role description.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Creates an instance of Identity and copies the properties of the same name from the object parameter. 
        /// </summary>
        /// <param name="source">The object to copy.</param>
        /// <returns></returns>
        public static IdentityRoleEdit Create(object source)
        {
            var result = new IdentityRoleEdit();

            result.CopyFrom(source);
            return result;
        }
    }
}
#endif
//MdEnd
