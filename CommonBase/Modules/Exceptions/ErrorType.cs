//@CodeCopy
//MdStart
namespace CommonBase.Modules.Exceptions
{
    public static partial class ErrorType
    {
#if ACCOUNT_ON
        public static int InitAppAccess = 10;
        public static int InvalidAccount = 20;
        public static int InvalidIdentityName = 30;
        public static int InvalidPasswordSyntax = 40;

        public static int InvalidToken = 50;
        public static int InvalidSessionToken = 60;
        public static int InvalidJsonWebToken = 70;

        public static int InvalidEmail = 80;
        public static int InvalidPassword = 90;
        public static int NotLogedIn = 100;
        public static int NotAuthorized = 110;
        public static int AuthorizationTimeOut = 120;

#if ACCESSRULES_ON
        public static int InvalidAccessRuleEntityValue = 130;
        public static int InvalidAccessRuleAccessValue = 140;
        public static int InvalidAccessRuleAlreadyExits = 150;

        public static int AccessRuleViolationCanNotCreated = 160;
        public static int AccessRuleViolationCanNotRead = 170;
        public static int AccessRuleViolationCanNotChanged = 180;
        public static int AccessRuleViolationCanNotDeleted = 190;
#endif
#endif

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static int InvalidId = 200;
        public static int InvalidPageSize = 210;

        public static int InvalidEntityInsert = 220;
        public static int InvalidEntityUpdate = 230;
        public static int InvalidEntityContent = 240;

        public static int InvalidControllerType = 250;
        public static int InvalidControllerObject = 260;

        public static int InvalidFacadeType = 270;
        public static int InvalidFacadeObject = 280;

        public static int InvalidOperation = 290;
        public static int EmailWasNotSent = 300;
        public static int InvalidConfirmation = 310;
#pragma warning restore CA2211 // Non-constant fields should not be visible
    }
}
//MdEnd
