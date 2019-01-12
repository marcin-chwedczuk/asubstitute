using System;
using ASubstitute.Buildin.Behaviours;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Behaviours {
        public static T Returns<T>(this T obj, T valueToReturn) {
            ThreadLocalContext.RegisterBehaviour(
                new ReturnMethodBehaviour(valueToReturn));

            return default(T);
        }

        public static T Throws<T>(this T returnedValue, Exception exceptionToThrow) {
            ThreadLocalContext.RegisterBehaviour(
                new ThrowMethodBehaviour(exceptionToThrow));

            return default(T);
        }
    }
}