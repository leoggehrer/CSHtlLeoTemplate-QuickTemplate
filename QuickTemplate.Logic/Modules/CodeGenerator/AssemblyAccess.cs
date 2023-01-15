//@BaseCode
//MdStart
using System.Reflection;

namespace QuickTemplate.Logic.Modules.CodeGenerator
{
    public static class AssemblyAccess
    {
        public static Type[] AllTypes
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();

                return assembly.GetTypes();
            }
        }
        public static Type[] EntityTypes => AllTypes.Where(t => string.IsNullOrEmpty(t.FullName) == false 
                                                             && t.FullName.Contains(".Entities.")).ToArray();
    }
}
//MdEnd
