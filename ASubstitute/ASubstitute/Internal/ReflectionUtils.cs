using System;

namespace ASubstitute.Internal {
    public static class ReflectionUtils {
        public static object CreateDefaultValue(Type type) {
            var result = type.IsValueType && !IsVoid(type)
                ? Activator.CreateInstance(type)
                : null;

            return result;
        }

        private static bool IsVoid(Type type)
            => (type == typeof(void));
    }
}