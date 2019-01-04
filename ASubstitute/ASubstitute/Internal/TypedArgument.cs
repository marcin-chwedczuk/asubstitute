using System;

namespace ASubstitute.Internal {
    public class TypedArgument {
        public Type Type { get; }
        public object Value { get; }

        public TypedArgument(Type type, object value) {
            Type = type;
            Value = value;
        }
    }

}