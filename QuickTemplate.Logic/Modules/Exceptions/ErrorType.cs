//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Modules.Exceptions
{
    public enum ErrorType : int
    {
#if ACCOUNT_ON
        InitAppAccess = 10,
        InvalidAccount = 20,
        InvalidIdentityName = 30,
        InvalidPasswordSyntax = 40,

        InvalidToken = 50,
        InvalidSessionToken = 60,
        InvalidJsonWebToken = 70,

        InvalidEmail = 80,
        InvalidPassword = 90,
        NotLogedIn = 100,
        NotAuthorized = 110,
        AuthorizationTimeOut = 120,

#if ACCESSRULES_ON
        InvalidAccessRuleEntityValue = 130,
        InvalidAccessRuleAccessValue = 140,
        InvalidAccessRuleAlreadyExits = 150,

        AccessRuleViolationCanNotCreated = 160,
        AccessRuleViolationCanNotRead = 170,
        AccessRuleViolationCanNotChanged = 180,
        AccessRuleViolationCanNotDeleted = 190,
#endif
#endif
        InvalidId = 200,
        InvalidPageSize = 210,

        InvalidEntityInsert = 220,
        InvalidEntityUpdate = 230,
        InvalidEntityContent = 240,

        InvalidControllerType = 250,
        InvalidControllerObject = 260,

        InvalidFacadeType = 270,
        InvalidFacadeObject = 280,

        InvalidOperation = 290,
        EmailWasNotSent = 300,
        InvalidConfirmation = 310,
    }
}
//MdEnd