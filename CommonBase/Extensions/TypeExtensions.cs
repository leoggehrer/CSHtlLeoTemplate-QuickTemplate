//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace CommonBase.Extensions
{
    public static partial class TypeExtensions
    {
        public partial class Extend
        {
            public Type? Type { get; set; }
            public List<Extend> Extends { get; set; } = new();
        }
        public static void CheckInterface(this Type? type, string argName)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsInterface == false)
                throw new ArgumentException($"The parameter '{argName}' must be an interface.");
        }
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static bool IsGenericCollectionType(this Type type)
        {
            return type.FullName!.StartsWith($"{nameof(System)}.{nameof(System.Collections)}.{nameof(System.Collections.Generic)}");
        }
        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            return
               type.IsValueType ||
               type.IsPrimitive ||
               new[]
               {
               typeof(String),
               typeof(Decimal),
               typeof(DateTime),
               typeof(DateTimeOffset),
               typeof(TimeSpan),
               typeof(Guid)
               }.Contains(type) ||
               (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        public static Type? GetUnderlyingType(this MemberInfo member)
        {
            return member.MemberType switch
            {
                MemberTypes.Event => ((EventInfo)member).EventHandlerType,
                MemberTypes.Field => ((FieldInfo)member).FieldType,
                MemberTypes.Method => ((MethodInfo)member).ReturnType,
                MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                _ => throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"),
            };
        }
        public static bool IsNumericType(this Type type)
        {
            var result = false;
            var checkType = type;

            if (type.IsNullableType())
            {
                checkType = Nullable.GetUnderlyingType(type);
            }

            switch (Type.GetTypeCode(checkType))
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    result = true;
                    break;
            }
            return result;
        }
        public static bool IsFloatingPointType(this Type type)
        {
            var result = false;
            var checkType = type;

            if (type.IsNullableType())
            {
                checkType = Nullable.GetUnderlyingType(type);
            }

            switch (Type.GetTypeCode(checkType))
            {
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    result = true;
                    break;
            }
            return result;
        }
        public static string GetSourceTypeName(this Type type)
        {
            var result = type.Name;

            if (type.IsGenericType)
            {
                result = result.Substring(0, "`");
                result += "<";
                for (int i = 0; i < type.GetGenericArguments().Length; i++)
                {
                    if (i > 0)
                    {
                        result += ", ";
                    }
                    result += GetSourceTypeName(type.GetGenericArguments()[i]);
                }
                result += ">";
            }
            return result;
        }

        public static Extend GetInterfaceHierarchy(this Type type)
        {
            var result = new Extend { Type = type };

            static void GetInterfaceHierarchyRec(Extend root)
            {
                root.Extends = root.Type!.GetAllInterfaces()
                                            .Select(e => new Extend { Type = e })
                                            .ToList();
                foreach (var item in root.Extends)
                {
                    GetInterfaceHierarchyRec(item);
                }
            }

            if (type.IsInterface)
            {
                GetInterfaceHierarchyRec(result);
            }
            return result;
        }
        public static IEnumerable<Type> GetClassHierarchy(this Type type)
        {
            var result = new List<Type>();

            if (type.IsClass)
            {
                var run = type.BaseType;

                result.Add(type);
                while (run != null && run.Name.Equals("object", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    result.Add(run);
                    run = run.BaseType;
                }
            }
            return result;
        }
        public static IEnumerable<Type> GetAllInterfaces(this Type type)
        {
            static void GetBaseInterfaces(Type type, List<Type> interfaces)
            {
                var run = type;

                while (run != null)
                {
                    foreach (var item in run.GetInterfaces())
                    {
                        if (interfaces.Contains(item) == false)
                        {
                            interfaces.Add(item);
                        }
                        GetBaseInterfaces(item, interfaces);
                    }
                    run = run.BaseType;
                }
            }
            var result = new List<Type>();

            GetBaseInterfaces(type, result);
            return result;
        }
        public static IEnumerable<Type> GetDeclaredInterfaces(this Type type)
        {
            var run = type.BaseType;
            IEnumerable<Type> result = type.GetInterfaces();

            foreach (var item in result)
            {
                result = result.Except(item.GetAllInterfaces());
            }
            while (run != null)
            {
                result = result.Except(run.GetInterfaces());
                foreach (var item in result)
                {
                    result = result.Except(item.GetAllInterfaces());
                }
                run = run.BaseType;
            }
            return result;
        }
        public static IEnumerable<Type> GetRelations(this Type type)
        {
            return type.GetRelations(0);
        }
        public static IEnumerable<Type> GetRelations(this Type type, int maxDeep)
        {
            static IEnumerable<Type> GetRelationsRec(Type type, List<Type> relations, int maxDeep, int deep)
            {
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                foreach (var item in type.GetFields(bindingFlags))
                {
                    if (item.FieldType.IsValueType == false)
                    {
                        if (item.FieldType.IsGenericType
                            && item.FieldType.GetGenericArguments().Any(e => (e.IsClass || e.IsInterface) && e != typeof(string)))
                        {
                            relations.Add(item.FieldType);
                            if (maxDeep < deep)
                            {
                                GetRelationsRec(item.FieldType, relations, maxDeep, deep + 1);
                            }
                        }
                        else if ((item.FieldType.IsClass || item.FieldType.IsInterface) && item.FieldType != typeof(string))
                        {
                            relations.Add(item.FieldType);
                            if (maxDeep < deep)
                            {
                                GetRelationsRec(item.FieldType, relations, maxDeep, deep + 1);
                            }
                        }
                    }
                }
                return relations;
            }
            var result = new List<Type>();

            return GetRelationsRec(type, result, maxDeep, 0);
        }
        public static IEnumerable<Type> GetBaseInterfaces(this Type type)
        {
            static void GetBaseInterfaces(Type type, List<Type> interfaces)
            {
                foreach (var item in type.GetInterfaces())
                {
                    if (interfaces.Contains(item) == false)
                    {
                        interfaces.Add(item);
                    }
                    GetBaseInterfaces(item, interfaces);
                }
            }
            var result = new List<Type>();

            GetBaseInterfaces(type, result);
            return result;
        }
        public static IEnumerable<FieldInfo> GetAllClassFields(this Type type)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var result = new List<FieldInfo>();

            if (type.IsClass)
            {
                var run = type.BaseType;

                result.AddRange(type.GetFields(bindingFlags));
                while (run != null && run.Name.Equals("object", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    result.AddRange(run.GetFields(bindingFlags));
                    run = run.BaseType;
                }
            }
            return result;
        }

        public static PropertyInfo? GetInterfaceProperty(this Type? type, string name)
        {
            type.CheckArgument(nameof(type));

            return type?.GetAllInterfacePropertyInfos()
                        .SingleOrDefault(p => p.Name.Equals(name));
        }

        public static Dictionary<string, PropertyItem> GetAllTypeProperties(this Type typeObject)
        {
            var propertyItems = new Dictionary<string, PropertyItem>();

            GetAllTypePropertiesRec(typeObject, propertyItems);
            return propertyItems;
        }
        private static void GetAllTypePropertiesRec(Type typeObject, Dictionary<string, PropertyItem> propertyItems)
        {
            if (typeObject.BaseType != null
                && typeObject.BaseType != typeof(object))
            {
                GetAllTypePropertiesRec(typeObject.BaseType, propertyItems);
            }

            foreach (var property in typeObject.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                               .OrderBy(p => p.MetadataToken))
            {
                var propertyItem = new PropertyItem(property);

                if (propertyItems.ContainsKey(property.Name) == false)
                {
                    propertyItems.Add(property.Name, propertyItem);
                }
                else
                {
                    propertyItems[property.Name] = propertyItem;
                }
                // Bei rekursiven Strukturen kommt es hier zu einem Absturz!!!
                //if (propertyItem.IsComplexType)
                //{
                //    GetAllTypePropertiesRec(propertyItem.PropertyInfo.PropertyType, propertyItem.PropertyItems);
                //}
            }
        }

        public static IEnumerable<PropertyInfo> GetAllPropertyInfos(this Type type)
        {
            type.CheckArgument(nameof(type));

            return type.IsInterface == false ? type.GetAllTypePropertyInfos() : type.GetAllInterfacePropertyInfos();
        }
        public static IEnumerable<PropertyInfo> GetAllTypePropertyInfos(this Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<PropertyInfo>();

            foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                result.Add(item);
            }
            return result;
        }

        public static IEnumerable<PropertyInfo> GetAllInterfacePropertyInfos(this Type type)
        {
            var result = new List<PropertyInfo>();

            if (type.GetTypeInfo().IsInterface)
            {
                GetAllInterfacePropertyInfosRec(type, result);
            }
            else
            {
                foreach (var item in type.GetInterfaces())
                {
                    GetAllInterfacePropertyInfosRec(item, result);
                }
            }
            return result;
        }
        private static void GetAllInterfacePropertyInfosRec(Type type, List<PropertyInfo> properties)
        {
            foreach (var item in type.GetProperties())
            {
                if (properties.Find(p => p.Name.Equals(item.Name)) == null)
                {
                    properties.Add(item);
                }
            }
            foreach (var item in type.GetInterfaces())
            {
                GetAllInterfacePropertyInfosRec(item, properties);
            }
        }

        public static bool IsGenericTypeOf(this Type type, Type genericType)
        {
            Type? instanceType = type;

            while (instanceType != null)
            {
                if (instanceType.IsGenericType &&
                    instanceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                instanceType = instanceType.BaseType;
            }
            return false;
        }

        public static string DeclaringName(this PropertyInfo source) => $"{source.DeclaringType!.Name}.{source.Name}";
        public static bool IsNullable(this PropertyInfo source) => IsNullableHelper(source.PropertyType, source.DeclaringType, source.CustomAttributes);
        public static bool IsNullable(this FieldInfo source) => IsNullableHelper(source.FieldType, source.DeclaringType, source.CustomAttributes);
        public static bool IsNullable(this ParameterInfo source) => IsNullableHelper(source.ParameterType, source.Member, source.CustomAttributes);
        private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
        {
            if (memberType.IsValueType)
                return Nullable.GetUnderlyingType(memberType) != null;

            var nullable = customAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;

                    if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte)args[0].Value! == 2;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte)attributeArgument.Value! == 2;
                }
            }

            for (var type = declaringType; type != null; type = type.DeclaringType)
            {
                var context = type.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");

                if (context != null &&
                    context.ConstructorArguments.Count == 1 &&
                    context.ConstructorArguments[0].ArgumentType == typeof(byte))
                {
                    return (byte)context.ConstructorArguments[0].Value! == 2;
                }
            }
            // Couldn't find a suitable attribute
            return false;
        }
    }
}
//MdEnd
