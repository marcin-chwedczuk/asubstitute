using System;
using ASubstitute.Api.Assertions;

namespace ASubstitute.Internal {
    public class TypedArgument : ITypedArgument {
        public Type Type { get; }
        public object Value { get; }

        public bool HasDefaultValue
            => ReflectionUtils.IsDefaultValue(Type, Value);

        public TypedArgument(Type type, object value) {
            Type = type;
            Value = value;
        }

        public void Deconstruct(out Type type, out object value) {
            type = Type;
            value = Value;
        }
    }
}