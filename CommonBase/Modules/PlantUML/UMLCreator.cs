//@BaseCode
//MdStart
namespace CommonBase.Modules.PlantUML
{
    using CommonBase.Extensions;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    public static class UMLCreator
    {
        #region skinparam
        public static IEnumerable<string> DefaultClassSkinparam => new[] {
                                                                            "skinparam class {",
                                                                            " BackgroundColor whitesmoke",
                                                                            " ArrowColor grey",
                                                                            " BorderColor darkgrey",
                                                                            "}",
                                                                         };
        public static IEnumerable<string> DefaultObjectSkinparam => new[] {
                                                                            "skinparam object {",
                                                                            " BackgroundColor white",
                                                                            " ArrowColor grey",
                                                                            " BorderColor darkgrey",
                                                                            "}",
                                                                         };
        #endregion skinparam

        #region diagram creators
        public static IEnumerable<string> CreateClassDiagram(DiagramCreationFlags diagramCreationFlags, params Type[] types)
        {
            var result = new List<string>();
            var allTypes = new List<Type>(types);

            if ((diagramCreationFlags & DiagramCreationFlags.TypeExtends) > 0)
            {
                foreach (var type in allTypes.Clone().Where(t => t.IsClass))
                {
                    allTypes.AddRange(type.GetClassHierarchy().Where(e => allTypes.Contains(e) == false));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ImplementedInterfaces) > 0)
            {
                foreach (var type in allTypes.Clone())
                {
                    allTypes.AddRange(type.GetDeclaredInterfaces().Where(e => allTypes.Contains(e) == false));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.InterfaceExtends) > 0)
            {
                foreach (var type in allTypes.Clone().Where(t => t.IsInterface))
                {
                    allTypes.AddRange(type.GetClassHierarchy().Where(e => allTypes.Contains(e) == false));
                }
            }

            result.AddRange(CreateTypeDefinitions(allTypes, diagramCreationFlags));

            if ((diagramCreationFlags & DiagramCreationFlags.TypeExtends) > 0)
            {
                result.AddRange(CreateTypeHirarchies(allTypes.Where(t => t.IsClass)));
            }

            if ((diagramCreationFlags & DiagramCreationFlags.InterfaceExtends) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsInterface))
                {
                    var extend = type.GetInterfaceHierarchy();

                    extend.Extends.ForEach(e => result.AddRange(CreateTypeHierachy(new[] { extend.Type!, e.Type! })));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ImplementedInterfaces) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsClass))
                {
                    result.AddRange(CreateTypeImplements(type));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ClassRelations) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsClass))
                {
                    result.AddRange(CreateTypeRelations(type, 0));
                }
            }
            return result;
        }
        static string CreateObjectName(Object obj) => $"{obj.GetType().Name}_{obj.GetHashCode()}";
        static string CreateCollectionName(Object obj) => $"Colletion_{obj.GetHashCode()}";
        public static IEnumerable<string> CreateObjectDiagram(int maxDeep, params Object[] objects)
        {
            var result = new List<string>();
            var createdObjects = new List<object>();
            void CreateMapStateRec(Object[] objects, List<string> lines, int deep)
            {
                static void CreateMapObjectState(Object obj, List<string> lines)
                {
                    lines.Add($"map {CreateObjectName(obj)} " + "{");
                    lines.AddRange(CreateObjectState(obj).SetIndent(1));
                    lines.Add("}");
                }
                static void CreateMapCollectionState(IEnumerable collection, List<string> lines)
                {
                    lines.Add($"map {CreateCollectionName(collection)} " + "{");
                    lines.AddRange(CreateCollectionState(collection).SetIndent(1));
                    lines.Add("}");
                }

                foreach (var obj in objects)
                {
                    if (createdObjects.Contains(obj) == false)
                    {
                        createdObjects.Add(obj);
                        CreateMapObjectState(obj, lines);

                        if (deep + 1 <= maxDeep)
                        {
                            var relations = obj.GetType().GetRelations();
                            var relationFieldInfos = obj.GetType().GetAllClassFields()
                                                        .Where(fi => relations.Any(r => r == fi.FieldType));

                            foreach (var relFieldInfo in relationFieldInfos)
                            {
                                var value = GetFieldValue(obj, relFieldInfo);

                                if (value is IEnumerable collection)
                                {
                                    var counter = 0;

                                    CreateMapCollectionState(collection, lines);
                                    lines.Add($"{CreateObjectName(obj)}::{GetFieldName(relFieldInfo)} --> {CreateCollectionName(value)}");
                                    if (deep + 2 <= maxDeep)
                                    {
                                        foreach (var item in collection)
                                        {
                                            if (item != null)
                                            {
                                                CreateMapStateRec(new[] { item }, lines, deep + 2);
                                                lines.Add($"{CreateCollectionName(collection)}::{counter} --> {CreateObjectName(item)}");
                                            }
                                            counter++;
                                        }
                                    }
                                }
                                else if (value != null)
                                {
                                    CreateMapStateRec(new[] { value }, lines, deep + 1);
                                    lines.Add($"{CreateObjectName(obj)}::{GetFieldName(relFieldInfo)} --> {CreateObjectName(value)}");
                                }
                            }
                        }
                    }
                }
            }
            CreateMapStateRec(objects, result, 0);
            return result;
        }
        #endregion diagram creators

        #region diagram helpers
        public static IEnumerable<string> CreateTypeDefinition(Type type, DiagramCreationFlags diagramCreationFlags)
        {
            var result = new List<string>();

            if (type.IsEnum)
            {
                result.Add($"enum {type.Name} #light" +
                    $"blue " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.EnumMembers) > 0)
                {
                    foreach (var item in Enum.GetValues(type))
                    {
                        result.Add($" {item}");
                    }
                }
                result.Add("}");
            }
            else if (type.IsClass)
            {
                var prefix = type.IsPublic ? "+" : String.Empty;

                if (type.IsAbstract)
                    result.Add($"{prefix}abstract class {type.Name} #white " + "{");
                else
                    result.Add($"{prefix}class {type.Name} #whitesmoke " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.ClassMembers) > 0)
                {
                    result.AddRange(CreateItemMembers(type).SetIndent(1));
                }
                result.Add("}");
            }
            else if (type.IsInterface)
            {
                result.Add($"interface {type.Name} #lightgrey " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.InterfaceMembers) > 0)
                {
                    result.AddRange(CreateItemMembers(type).SetIndent(1));
                }
                result.Add("}");
            }
            return result;
        }
        public static IEnumerable<string> CreateTypeHirarchies(IEnumerable<Type> types)
        {
            var result = new List<string>();

            foreach (var item in CreateDiagramHirarchies(types))
            {
                result.AddRange(CreateTypeHierachy(item));
            }
            return result;
        }
        public static IEnumerable<string> CreateTypeHierachy(IEnumerable<Type> types)
        {
            var result = new List<string>();
            var typeArray = types.ToArray();

            for (int i = 0; i < typeArray.Length - 1; i++)
            {
                if (typeArray[i + 1].IsInterface || typeArray[i].IsInterface)
                {
                    result.Add($"{typeArray[i + 1].Name} <|.. {typeArray[i].Name}");
                }
                else
                {
                    result.Add($"{typeArray[i + 1].Name} <|-- {typeArray[i].Name}");
                }
            }
            return result;
        }
        public static IEnumerable<string> CreateTypeRelations(Type type, int deep)
        {
            var test = new int[10];
            var result = new List<string>();

            foreach (var item in type.GetRelations(deep))
            {
                if (item.IsArray)
                {
                    var elemType = item.UnderlyingSystemType;

                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} --> \"0..1\" {elemType.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} --> \"1\" {elemType.Name}");
                    }
                }
                else if (item.IsGenericCollectionType())
                {
                    var elemType = item.GetGenericArguments()[0];

                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} \"*\" o-- \"0..1\" {elemType.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} \"*\" *-- \"1\" {elemType.Name}");
                    }
                }
                else
                {
                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} --> \"0..1\" {item.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} --> \"1\" {item.Name}");
                    }
                }
            }
            return result;
        }
        public static IEnumerable<string> CreateTypeImplements(Type type)
        {
            var result = new List<string>();
            var lolypop = "()-";

            foreach (var typeInfo in type.GetDeclaredInterfaces())
            {
                result.Add($"{typeInfo.Name} {lolypop} {type.Name}");
            }
            return result;
        }
        public static IEnumerable<string> CreateTypeDefinitions(IEnumerable<Type> types, DiagramCreationFlags diagramCreationFlags)
        {
            var result = new List<string>();

            foreach (var type in types)
            {
                result.AddRange(CreateTypeDefinition(type, diagramCreationFlags));
            }
            return result;
        }
        public static IEnumerable<string> CreateItemMembers(Type type)
        {
            var counter = 0;
            var result = new List<string>();
            var bindingFlags = default(BindingFlags);

            #region fields
            bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in type.GetFields(bindingFlags))
            {
                counter++;
                result.Add($"{(item.IsPublic ? "+" : "-")}" + " {static}" + $"{item.FieldType.GetSourceTypeName()} {GetFieldName(item)}");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetFields(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix}" + " " + $"{item.FieldType.GetSourceTypeName()} {GetFieldName(item)}");
            }
            if (counter > 0)
                result.Add("---");
            #endregion fields

            #region properties
            counter = 0;
            bindingFlags = BindingFlags.Static | BindingFlags.Public;
            foreach (var item in type.GetProperties(bindingFlags))
            {
                if (item.CanRead)
                {
                    counter++;
                    result.Add(" + {static}" + $"{item.PropertyType.GetSourceTypeName()} get{item.Name}()");
                }
                if (item.CanWrite)
                {
                    counter++;
                    result.Add(" + {static}" + $"set{item.Name}({item.PropertyType.GetSourceTypeName()} value)");
                }
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetProperties(bindingFlags))
            {
                if (item.CanRead)
                {
                    counter++;
                    result.Add($" + {item.PropertyType.GetSourceTypeName()} get{item.Name}()");
                }
                if (item.CanWrite)
                {
                    counter++;
                    result.Add($" + set{item.Name}({item.PropertyType.GetSourceTypeName()} value)");
                }
            }
            if (counter > 0)
                result.Add("---");
            #endregion

            #region methoden
            counter = 0;
            bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in type.GetMethods(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix} " + "{static}" + $"{item.ReturnType.GetSourceTypeName()} {item.Name}({GetParameters(item)})");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetMethods(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix} {item.ReturnType.GetSourceTypeName()} {item.Name}({GetParameters(item)})");
            }
            //if (counter > 0)
            //    result.Add("---");
            #endregion methoden
            return result;
        }
        public static IEnumerable<string> CreateObjectState(Object obj)
        {
            var counter = 0;
            var result = new List<string>();
            var bindingFlags = default(BindingFlags);

            #region fields
            bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in obj.GetType().GetFields(bindingFlags))
            {
                counter++;
               // result.Add("{static}" + $"{item.FieldType.Name} {GetFieldName(item)} => {GetStateValue(obj, item)}");
                result.Add("{static}" + $"{GetFieldName(item)} => {GetStateValue(obj, item)}");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            foreach (var item in obj.GetType().GetAllClassFields())
            {
                counter++;
//                result.Add($"{item.FieldType.Name} {GetFieldName(item)} => {GetStateValue(obj, item)}");
                result.Add($"{GetFieldName(item)} => {GetStateValue(obj, item)}");
            }
            //if (counter > 0)
            //    result.Add("---");
            #endregion fields

            return result;
        }
        public static IEnumerable<string> CreateCollectionState(IEnumerable collection)
        {
            var counter = 0;
            var result = new List<string>();

            foreach (var item in collection)
            {
                if (item != null)
                {
                    result.Add($"{counter++} => {item.GetType().Name}_{item.GetHashCode()}");
                }
                else
                {
                    result.Add($"{counter++} => null");
                }
            }
            return result;
        }

        #endregion diagram helpers

        #region helpers
        public static IEnumerable<IEnumerable<Type>> CreateDiagramHirarchies(IEnumerable<Type> types)
        {
            var result = new List<IEnumerable<Type>>();

            foreach (var type in types)
            {
                var classHirarchy = type.GetClassHierarchy();

                if (classHirarchy.Count() > 1)
                {
                    result.Add(classHirarchy);
                }
            }
            var calculatedHirarchies = new List<IEnumerable<Type>>();

            for (int i = 0; i < result.Count - 1; i++)
            {
                var commonSet = result[i];

                for (int j = i + 1; j < result.Count; j++)
                {
                    commonSet = commonSet.Intersect(result[j]);
                }

                if (commonSet.Count() > 1)
                {
                    for (int j = 0; j < result.Count; j++)
                    {
                        var currentSet = result[j];
                        var exceptSet = currentSet.Except(commonSet);

                        if (commonSet.All(e => currentSet.Any(c => e == c)) && exceptSet.Any())
                        {
                            var intersectSet = result[j].Intersect(new[] { commonSet.First() });
                            var createSet = exceptSet.Union(intersectSet);

                            calculatedHirarchies.Add(createSet);
                        }
                        else
                        {
                            calculatedHirarchies.Add(result[j]);
                        }
                    }
                }
                else
                {
                    calculatedHirarchies.AddRange(result);
                }
                result.Clear();
                result.AddRange(calculatedHirarchies);
                calculatedHirarchies.Clear();
            }
            return result;
        }
        public static string GetFieldName(FieldInfo fieldInfo)
        {
            var result = string.Empty;

            if (fieldInfo.Name.Contains("k__BackingField"))
            {
                result = "_" + fieldInfo.Name.Betweenstring("<", ">");
                result = result.Substring(0, 2).ToLower() + result.Substring(2);
            }
            else
            {
                result = fieldInfo.Name;
            }
            return result;
        }
        public static string GetParameters(MethodInfo methodInfo)
        {
            var counter = 0;
            var result = new StringBuilder();

            foreach (var item in methodInfo.GetParameters())
            {
                if (counter++ > 0)
                    result.Append(", ");

                result.Append($"{item.ParameterType.Name} {item.Name}");
            }
            return result.ToString();
        }
        public static Object? GetFieldValue(Object obj, FieldInfo fieldInfo)
        {
            var value = default(Object);

            if (fieldInfo.IsStatic)
            {
                value = fieldInfo.GetValue(null);
            }
            else
            {
                value = fieldInfo.GetValue(obj);
            }
            return value;
        }
        public static string GetStateValue(Object obj, FieldInfo fieldInfo)
        {
            return GetStateValue(obj, fieldInfo, 15);
        }
        public static string GetStateValue(Object obj, FieldInfo fieldInfo, int maxLength)
        {
            var result = string.Empty;
            var value = GetFieldValue(obj, fieldInfo);

            if (fieldInfo.FieldType.IsValueType)
            {
                result = value?.ToString() ?? string.Empty;
            }
            else if (fieldInfo.FieldType == typeof(string))
            {
                result = $"\"{value}\"";
            }
            else if (value == null)
            {
                result = "null";
            }
            else
            {
                result = $"_{value.GetHashCode()}";
            }
            return result.Length > maxLength - 3 ? result.Substring(0, maxLength - 2) + "..." : result;
        }
        #endregion helpers
    }
}
//MdEnd