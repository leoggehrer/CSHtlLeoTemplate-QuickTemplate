//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents a filter model for IdentityUser objects.
    /// </summary>
    public partial class IdentityUserFilter : Models.View.IFilterModel
    {
        /// <summary>
        /// Static constructor for the IdentityUserFilter class.
        /// </summary>
        /// <remarks>
        /// This constructor is automatically called before any static member of the class is accessed or any instance of the class is created.
        /// It is used to initialize any static members of the class or perform any necessary setup operations.
        /// </remarks>
        /// /
        static IdentityUserFilter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Method called when the class is being constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// It can be implemented in different parts of the class using the "partial" keyword.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserFilter"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the <see cref="Constructing"/> and <see cref="Constructed"/> methods.
        /// </remarks>
        public IdentityUserFilter()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method should be implemented in a partial class in order to add any additional logic or initialization after the object is constructed.
        /// </remarks>
        /// <seealso cref="OtherMethod()"/>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the identity ID of the property.
        /// </summary>
        /// <value>The identity ID of the property.</value>
        public IdType? IdentityId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        public string? Firstname
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the last name of a person.
        /// </summary>
        /// <value>
        /// The last name of a person.
        /// </value>
        public string? Lastname
        {
            get;
            set;
        }
        /// <summary>
        /// Gets a value indicating whether the entity has any valid properties with values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entity has any valid properties with values; otherwise, <c>false</c>.
        /// </value>
        public bool HasEntityValue => IdentityId != null || Firstname != null || Lastname != null;
        private bool show = true;
        /// <summary>
        /// Gets or sets a value indicating whether the show property is true or false.
        /// </summary>
        /// <value>True if the show property is true; otherwise, false.</value>
        public bool Show => show;
        /// <summary>
        /// Creates a predicate for filtering entities based on specified criteria.
        /// </summary>
        /// <returns>A string representation of the created predicate.</returns>
        public string CreateEntityPredicate()
        {
            var result = new System.Text.StringBuilder();
            if (IdentityId != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(IdentityId != null && IdentityId == {IdentityId})");
            }
            if (Firstname != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Firstname != null && Firstname.Contains(\"{Firstname}\"))");
            }
            if (Lastname != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Lastname != null && Lastname.Contains(\"{Lastname}\"))");
            }
            return result.ToString();
        }
        /// <summary>
        /// Converts the object to its string representation.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            if (IdentityId != null)
            {
                sb.Append($"IdentityId: {IdentityId}");
            }
            if (string.IsNullOrEmpty(Lastname) == false)
            {
                sb.Append($"Lastname: {Lastname} ");
            }
            if (string.IsNullOrEmpty(Firstname) == false)
            {
                sb.Append($"Firstname: {Firstname} ");
            }
            return sb.ToString();
        }
    }
}
#endif
//MdEnd

