using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    public static class ReflectionUtils {
        public static object CreateDefaultValue(Type type) {
            if (IsReferenceType(type)) {
                return null;
            }

            var result = IsVoid(type)
                ? null
                : Activator.CreateInstance(type);

            return result;
        }

        private static bool IsReferenceType(Type type)
            => !type.IsValueType;

        private static bool IsVoid(Type type)
            => (type == typeof(void));

        public static bool IsDefaultValue(Type type, object value) {
            if (IsReferenceType(type)) {
                return (value is null);
            }

            object defaultValue = CreateDefaultValue(type);
            return object.Equals(defaultValue, value);
        }

        private static readonly IReadOnlyDictionary<Type, string> _csharpNameByType = new Dictionary<Type, string> {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(string)] = "string",
            [typeof(decimal)] = "decimal"
        };

        public static string GetCSharpNameOf(Type type) {
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                return $"{GetCSharpNameOf(underlyingType)}?";

            if (_csharpNameByType.TryGetValue(type, out var csharpTypeName))
                return csharpTypeName;

            return type.Name;
        }
    }
}