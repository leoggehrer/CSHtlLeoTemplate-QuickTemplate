﻿//@CodeCopy
//MdStart
#if ACCOUNT_ON

namespace CommonBase.Security
{
    public static partial class PasswordRules
    {
        #region Class members
        static PasswordRules()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class members

        #region Rule definitions
        public static int MinimumLength => 6;
        public static int MaximumLength => 30;
        public static int MinLetterCount => 2;
        public static int MinUpperLetterCount => 1;
        public static int MinLowerLetterCount => 1;
        public static int MinSpecialLetterCount => 0;
        public static int MinDigitCount => 0;

        public const int MinLoginFails = 0;
        public const int MaxLoginFails = 20;

        private static int allowedLoginFails = 10;

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
