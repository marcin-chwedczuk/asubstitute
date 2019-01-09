using System;
using System.Collections.Generic;

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
    }
}