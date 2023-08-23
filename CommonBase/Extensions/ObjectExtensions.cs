//@BaseCode
//MdStart
using System.Reflection;

namespace CommonBase.Extensions
{
    public static partial class ObjectExtensions
    {
        #region Check arguments
        public static void CheckArgument(this object? source, string argName)
        {
            if (source == null)
                throw new ArgumentNullException(argName);
        }
        #endregion Check arguments

        #region Equal methods
        public static bool AreEqualProperties(this object source, object other, params string[] ignore)
        {
            static bool IsSimpleType(PropertyInfo propertyInfo)
            {
                var result = false;
                var underlingType = propertyInfo.GetUnderlyingType();

                if (underlingType != null)
                {
                    result = underlingType.IsSimpleType();
                }
                return result;
            }
            var sourceType = source.GetType();
            var otherType = other.GetType();
            var result = sourceType == otherType && source == other;

            if (sourceType == otherType)
            {
                var ignoreList = new List<string>(ignore);
                var unequalProperties =
                    from pi in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where pi != null && !ignoreList.Contains(pi.Name) && pi.GetIndexParameters().Length == 0 && IsSimpleType(pi)
                    let sourceValue = sourceType.GetProperty(pi.Name)?.GetValue(source, null)
                    let otherValue = otherType.GetProperty(pi.Name)?.GetValue(other, null)
                    where sourceValue != otherValue && (sourceValue == null || !sourceValue.Equals(otherValue))
                    select sourceValue;

                result = unequalProperties.Any() == false;
            }
            return result;
        }
        #endregion Equal methods

        #region CopyTo methods
        public static T CopyTo<T>(this object source) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, bool> filter) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, filter, null);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, string>? mapping) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, null, mapping);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, bool>? filter, Func<string, string>? mapping) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, filter, mapping);
            return target;
        }

        public static void CopyTo(this object source, object target)
        {
            CopyProperties(target, source);
        }
        public static void CopyTo(this object source, object target, Func<string, bool> filter)
        {
            CopyProperties(target, source, filter, null);
        }
        public static void CopyTo(this object source, object target, Func<string, string> mapping)
        {
            CopyProperties(target, source, null, mapping);
        }
        public static void CopyTo(this object source, object target, Func<string, bool> filter, Func<string, string> mapping)
        {
            CopyProperties(target, source, filter, mapping);
        }
        #endregion CobyTo methods

        #region CopyFrom methods
        public static void CopyFrom(this object target, object source)
        {
            if (source != null)
            {
                CopyProperties(target, source);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, bool> filter)
        {
            if (source != null)
            {
                CopyProperties(target, source, filter, null);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, string> mapping)
        {
            if (source != null)
            {
                CopyProperties(target, source, null, mapping);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, bool> filter, Func<string, string> mapping)
        {
            if (source != null)
            {
                CopyProperties(target, source, filter, mapping);
            }
        }
        #endregion CopyFrom methods

        #region CopyProperties methods
        public static void CopyProperties(object target, object source)
        {
            CopyProperties(target, source, null, null);
        }
        public static void CopyProperties(object target, object source, Func<string, bool>? filter, Func<string, string>? mapping)
        {
            Dictionary<string, PropertyItem> targetPropertyInfos = target.GetType().GetAllTypeProperties();
            Dictionary<string, PropertyItem> sourcePropertyInfos = source.GetType().GetAllTypeProperties();

            SetPropertyValues(target, source, filter, mapping, targetPropertyInfos, sourcePropertyInfos);
        }
        #endregion CopyProperties methods

#pragma warning disable IDE0060 // Remove unused parameter
        public static int CalculateHashCode(this object source, params object?[] items)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            static int CalculateHashCodeRec(object[] values)
            {
                var result = 0;

                if (values.Length == 1)
                {
                    result = HashCode.Combine(values[0]);
                }
                else if (values.Length == 2)
                {
                    result = HashCode.Combine(values[0], values[1]);
                }
                else if (values.Length == 3)
                {
                    result = HashCode.Combine(values[0], values[1], values[2]);
                }
                else if (values.Length == 4)
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3]);
                }
                else if (values.Length == 5)
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3], values[4]);
                }
                else if (values.Length == 6)
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5]);
                }
                else if (values.Length == 7)
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                }
                else if (values.Length == 8)
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
                }
                else
                {
                    result = HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]) + CalculateHashCodeRec(values.Skip(8).Take(values.Length - 8).ToArray());
                }
                return result;
            }
            var noneNullValues = items.Where(i => i != null).ToArray();

            return CalculateHashCodeRec(noneNullValues!);
        }
        private static void SetPropertyValues(object target, object source, Func<string, bool>? filter, Func<string, string>? mapping, Dictionary<string, PropertyItem> targetPropertyInfos, Dictionary<string, PropertyItem> sourcePropertyInfos)
        {
            filter ??= (n => true);
            mapping ??= (n => n);
            foreach (KeyValuePair<string, PropertyItem> propertyItemTarget in targetPropertyInfos)
            {
                if (sourcePropertyInfos.TryGetValue(mapping(propertyItemTarget.Value.PropertyInfo.Name), out var propertyItemSource))
                {
                    if (propertyItemSource.PropertyInfo.PropertyType == propertyItemTarget.Value.PropertyInfo.PropertyType
                        && ((propertyItemSource.CanReadAndIsPublic && propertyItemTarget.Value.CanWrite)
                            || (propertyItemSource.DeclaringType == propertyItemTarget.Value.DeclaringType && propertyItemSource.CanRead && propertyItemTarget.Value.CanWrite))
                        && (filter(propertyItemTarget.Value.PropertyInfo.Name)))
                    {
                        if (propertyItemSource.IsStringType)
                        {
                            object? value = propertyItemSource.PropertyInfo.GetValue(source);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value);
                        }
                        else if (propertyItemSource.IsArrayType)
                        {
                            object? value = propertyItemSource.PropertyInfo.GetValue(source);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value);
                        }
                        else if (propertyItemSource.PropertyInfo.PropertyType.IsValueType
                            && propertyItemTarget.Value.PropertyInfo.PropertyType.IsValueType)
                        {
                            object? value = propertyItemSource.PropertyInfo.GetValue(source);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value);
                        }
                        else if (propertyItemSource.IsComplexType)
                        {
                            object? value = propertyItemSource.PropertyInfo.GetValue(source);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value);
                            //object? srcValue = propertyItemSource.PropertyInfo.GetValue(source);
                            //object? tarValue = propertyItemTarget.Value.PropertyInfo.GetValue(target);

                            //if (srcValue != null && tarValue != null)
                            //{
                            //    SetPropertyValues(tarValue, srcValue, filter, mapping, propertyItemTarget.Value.PropertyItems, propertyItemSource.PropertyItems);
                            //}
                        }
                    }
                }
            }
        }
    }
}
//MdEnd
