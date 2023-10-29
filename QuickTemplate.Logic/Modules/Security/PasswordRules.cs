//@BaseCode
//MdStart
#if ACCOUNT_ON

namespace QuickTemplate.Logic.Modules.Security
{
    /// <summary>
    /// Provides a set of rules and properties for password validation.
    /// </summary>
    public static partial class PasswordRules
    {
        #region Class members
        /// <summary>
        ///     Initializes a new instance of the <see cref="PasswordRules"/> class.
        /// </summary>
        static PasswordRules()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any initialization tasks that need to be done when a class is constructed.
        /// </remarks>
        static partial void ClassConstructed();
        #endregion Class members
        
        #region Rule definitions
        /// <summary>
        /// Gets the minimum length of a value.
        /// </summary>
        /// <value>
        /// The minimum length.
        /// </value>
        public static int MinimumLength => 6;
        /// <summary>
        /// Gets the maximum length allowed for a string.
        /// </summary>
        /// <remarks>
        /// The maximum length is set to 30 characters.
        /// </remarks>
        /// <value>
        /// The maximum length allowed for a string.
        /// </value>
        public static int MaximumLength => 30;
        /// <summary>
        /// Gets the minimum letter count.
        /// </summary>
        /// <value>The minimum letter count is set to 2.</value>
        public static int MinLetterCount => 2;
        /// <summary>
        /// Gets the minimum count of uppercase letters allowed.
        /// </summary>
        /// <remarks>
        /// This property specifies the minimum number of uppercase letters that should be present.
        /// </remarks>
        /// <value>
        /// The minimum count of uppercase letters allowed.
        /// </value>
        public static int MinUpperLetterCount => 1;
        /// <summary>
        /// Gets the minimum count of lowercase letters.
        /// </summary>
        /// <value>The minimum count of lowercase letters.</value>
        public static int MinLowerLetterCount => 1;
        /// <summary>
        /// Gets the minimum count of special letters.
        /// </summary>
        /// <value>The minimum special letter count.</value>
        /// <remarks>
        /// This property represents the minimum count of special letters.
        /// The value is set to 0 by default.
        /// </remarks>
        public static int MinSpecialLetterCount => 0;
        /// <summary>
        /// Gets the minimum number of digits required for the property value.
        /// </summary>
        /// <value>The minimum number of digits required.</value>
        public static int MinDigitCount => 0;
        
        public const int MinLoginFails = 0;
        public const int MaxLoginFails = 20;
        
        private static int allowedLoginFails = 10;
        
        /// <summary>
        /// Gets or sets the maximum number of allowed login fails.
        /// </summary>
        /// <value>The maximum number of allowed login fails.</value>
        public static int AllowedLoginFails
        {
            get => allowedLoginFails;
            set
            {
                if (value >= MinLoginFails && value <= MaxLoginFails)
                {
                    allowedLoginFails = value;
                }
            }
        }
        
        #endregion Rule definitions
        /// <summary>
        /// Gets or sets the syntax roles for a string.
        /// </summary>
        /// <value>
        /// The syntax roles in the format:
        /// MinimumLength: {MinimumLength}
        /// MaximumLength: {MaximumLength}
        /// {newline}
        /// MinLetterCount: {MinLetterCount}
        /// MinDigitCount: {MinDigitCount}
        /// {newline}
        /// MinUpperLetterCount: {MinUpperLetterCount}
        /// MinLowerLetterCount: {MinLowerLetterCount}
        /// {newline}
        /// MinSpecialLetterCount: {MinSpecialLetterCount}
        /// </value>
        public static string SyntaxRoles => $"{nameof(MinimumLength)}: {MinimumLength} "
        + $"{nameof(MaximumLength)}: {MaximumLength} "
        + Environment.NewLine
        + $"{nameof(MinLetterCount)}: {MinLetterCount} "
        + $"{nameof(MinDigitCount)}: {MinDigitCount} "
        + Environment.NewLine
        + $"{nameof(MinUpperLetterCount)}: {MinUpperLetterCount} "
        + $"{nameof(MinLowerLetterCount)}: {MinLowerLetterCount} "
        + Environment.NewLine
        + $"{nameof(MinSpecialLetterCount)}: {MinSpecialLetterCount} ";
    }
}
#endif
//MdEnd

