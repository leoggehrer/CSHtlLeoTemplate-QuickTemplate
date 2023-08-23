//@CodeCopy
//MdStart
using System.Text;

namespace CommonBase.Modules.Exceptions
{
    public static partial class ErrorMessage
    {
        private static Dictionary<int, string> Messages { get; set; }
        static ErrorMessage()
        {
            ClassConstructing();
            Messages = new Dictionary<int, string>
            {
#if ACCOUNT_ON
				{ ErrorType.InitAppAccess, "The initialization of the app access is not permitted because an app access has already been initialized." },
				{ ErrorType.InvalidAccount, "Invalid identity or password." }
#endif
            };

            InitMessages();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        private static void InitMessages()
        {
            foreach (var item in typeof(ErrorType).GetFields())
            {
                var value = Convert.ToInt16(item.GetValue(null));

                if (Messages.ContainsKey(value) == false)
                {
                    var sb = new StringBuilder();
                    var error = item.Name ?? string.Empty;

                    foreach (var chr in error)
                    {
                        if (char.IsUpper(chr))
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(' ');
                            }
                            sb.Append(chr);
                        }
                        else
                            sb.Append(chr);
                    }
                    Messages.Add(value, sb.ToString());
                }
            }
        }

        public static string GetById(int errorId)
        {
            string result = string.Empty;

            if (Messages.ContainsKey(errorId))
            {
                result = Messages[errorId];
            }
            return result;
        }
    }
}
//MdEnd
