using System;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Behaviours {
        public static T Returns<T>(this T obj, T valueToReturn) {
            // TODO: Add public api for custom matchers (ThredLocalContext)
            ThreadLocalContext.RegisterBehaviour(
                new ReturnValueMethodBehaviour(valueToReturn));

            return default(T);
        }

        public static T Throws<T>(this T returnedValue, Exception exceptionToThrow) {
            ThreadLocalContext.RegisterBehaviour(
                new ThrowExceptionMethodBehaviour(exceptionToThrow));

            return default(T);
        }
    }
}