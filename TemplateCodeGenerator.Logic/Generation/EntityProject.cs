//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Contracts;
    internal sealed partial class EntityProject
    {
        public ISolutionProperties SolutionProperties { get; private set; }
        public string ProjectName => $"{SolutionProperties.SolutionName}{SolutionProperties.LogicPostfix}";
        public string ProjectPath => Path.Combine(SolutionProperties.SolutionPath, ProjectName);

        private EntityProject(ISolutionProperties solutionProperties)
        {
            SolutionProperties = solutionProperties;
        }
        public static EntityProject Create(ISolutionProperties solutionProperties)
        {
            return new(solutionProperties);
        }

        private IEnumerable<Type>? assemblyTypes;
        public IEnumerable<Type> AssemblyTypes
        {
            get
            {
                assemblyTypes = assemblyTypes ??= SolutionProperties.LogicAssemblyTypes;
                if (assemblyTypes == null)
                {
                    if (SolutionProperties.CompileLogicAssemblyFilePath.HasContent() && File.Exists(SolutionProperties.CompileLogicAssemblyFilePath))
                    {
                        var assembly = Assembly.LoadFile(SolutionProperties.CompileLogicAssemblyFilePath!);

                        if (assembly != null)
                        {
                            try
                            {
                                assemblyTypes = assembly.GetTypes();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error in {nameof(assemblyTypes)}: {ex.Message}");
                            }
                        }
                    }
                    if (assemblyTypes == null && SolutionProperties.LogicAssemblyFilePath.HasContent() && File.Exists(SolutionProperties.LogicAssemblyFilePath))
                    {
                        var assembly = Assembly.LoadFile(SolutionProperties.LogicAssemblyFilePath);

                        if (assembly != null)
                        {
                            try
                            {
                                assemblyTypes = assembly.GetTypes();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error in {nameof(assemblyTypes)}: {ex.Message}");
                            }
                        }
                    }
                }
                return assemblyTypes ?? Array.Empty<Type>();
            }
        }

        public IEnumerable<Type> EnumTypes => AssemblyTypes.Where(t => t.IsEnum);
        public IEnumerable<Type> InterfaceTypes => AssemblyTypes.Where(t => t.IsInterface);
        public IEnumerable<Type> EntityTypes => AssemblyTypes.Where(t => t.IsClass
                                                                      && t.IsAbstract == false
                                                                      && t.IsNested == false
                                                                      && t.Namespace != null
                                                                      && t.Namespace!.Contains($".{StaticLiterals.EntitiesFolder}")

                                                                      && t.FullName!.Contains($"{StaticLiterals.EntitiesFolder}.{StaticLiterals.AccountFolder}.") == false
                                                                      && t.FullName!.Contains($"{StaticLiterals.EntitiesFolder}.{StaticLiterals.LoggingFolder}.") == false
                                                                      && t.FullName!.Contains($"{StaticLiterals.EntitiesFolder}.{StaticLiterals.LoggingFolder}.") == false

                                                                      && t.Name.Equals(StaticLiterals.EntityObjectName) == false
                                                                      && t.Name.Equals(StaticLiterals.VersionEntityName) == false);
        public IEnumerable<Type> ServiceTypes => AssemblyTypes.Where(t => t.IsClass
                                                                       && t.IsAbstract == false
                                                                       && t.IsNested == false
                                                                       && t.Namespace != null
                                                                       && t.Namespace!.Contains($".{StaticLiterals.ServiceModelsFolder}")

                                                                       && t.Name.Equals(StaticLiterals.IdentityServiceName) == false
                                                                       && t.Name.Equals(StaticLiterals.VersionServiceName) == false);
        public static bool IsAccountEntity(Type type)
        {
            var result = type.FullName!.EndsWith($".{StaticLiterals.Account}.Identity")
                      || type.FullName!.EndsWith($".{StaticLiterals.Account}.IdentityXRole")
                      || type.FullName!.EndsWith($".{StaticLiterals.Account}.LoginSession")
                      || type.FullName!.EndsWith($".{StaticLiterals.Account}.Role")
                      || type.FullName!.EndsWith($".{StaticLiterals.Account}.SecureIdentity")
                      || type.FullName!.EndsWith($".{StaticLiterals.Account}.User");

            return result;
        }
        public static bool IsAccessEntity(Type type)
        {
            return type.FullName!.EndsWith($".{StaticLiterals.Access}.AccessRule");
        }
        public static bool IsLoggingEntity(Type type)
        {
            return type.FullName!.EndsWith($".{StaticLiterals.Logging}.ActionLog");
        }
        public static bool IsRevisionEntity(Type type)
        {
            return type.FullName!.EndsWith($".{StaticLiterals.Revision}.History");
        }
        public static bool IsNotAGenerationEntity(Type type)
        {
            return IsAccountEntity(type) || IsAccessEntity(type) || IsLoggingEntity(type) || IsRevisionEntity(type);
        }
    }
}
//MdEnd