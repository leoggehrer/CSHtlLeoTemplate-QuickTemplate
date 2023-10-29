//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents a filter model for identity information.
    /// Implements the <see cref="Models.View.IFilterModel" /> interface.
    /// </summary>
    public partial class IdentityFilter : Models.View.IFilterModel
    {
        ///<summary>
        /// Gets or sets the email address.
        ///</summary>
        ///<value>
        /// An optional string representing the email address.
        ///</value>
        ///<remarks>
        /// If the value is null, it means that no email address is specified.
        ///</remarks>
        public string? Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name property.
        /// </summary>
        /// <value>
        /// The name value. The default value is an empty string.
        /// </value>
        public string? Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets a value indicating whether the entity has a valid email or name.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entity has a valid email or name; otherwise, <c>false</c>.
        /// </value>
        public bool HasEntityValue => Email != null || Name != null;
        private bool show = true;
        /// <summary>
        /// Gets or sets a value indicating whether the show property is true or false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the show property is true; otherwise, <c>false</c>.
        /// </value>
        public bool Show => show;
        /// <summary>
        /// Creates an entity predicate based on the values of Email and Name.
        /// </summary>
        /// <returns>The entity predicate as a string.</returns>
        public string CreateEntityPredicate()
        {
            var result = new System.Text.StringBuilder();
            
            if (Email != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Email != null && Email.Contains(\"{Email}\"))");
            }
            if (Name != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Name != null && Name.Contains(\"{Name}\"))");
            }
            return result.ToString();
        }
        /// <summary>
        /// Overrides the default ToString() method to provide a custom string representation of the object.
        /// </summary>
        /// <returns>A string containing the email and name information.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            if (string.IsNullOrEmpty(Email) == false)
            {
                sb.Append($"Email: {Email} ");
            }
            if (string.IsNullOrEmpty(Name) == false)
            {
                sb.Append($"Name: {Name} ");
            }
            return sb.ToString();
        }
    }
}
#endif
//MdEnd
