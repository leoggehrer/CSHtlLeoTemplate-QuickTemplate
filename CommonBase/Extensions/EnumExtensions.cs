//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    public static class EnumExtensions
    {
        public static T FirstEnum<T>(this T src) where T : struct, Enum
        {
            if (typeof(T).IsEnum == false)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var values = (T[])Enum.GetValues(src.GetType());

            return values[0];
        }

        public static T NextEnum<T>(this T src) where T : struct, Enum
        {
            if (typeof(T).IsEnum == false)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var values = (T[])Enum.GetValues(src.GetType());
            var idx = Array.IndexOf<T>(values, src) + 1;

            return (values.Length == idx) ? values[0] : values[idx];
        }

        public static Dictionary<int, string> ToDictionary<T>(this T src) where T : struct, Enum
        {
            return Enum.GetValues(src.GetType())
               .Cast<T>()
               .ToDictionary(t => (int)(object)t, t => t.ToString());
        }
    }
}
//MdEnd
