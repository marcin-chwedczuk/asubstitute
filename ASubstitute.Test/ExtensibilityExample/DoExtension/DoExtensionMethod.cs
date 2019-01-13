using System;
using ASubstitute.Api;

namespace ASubstitute.Test.ExtensibilityExample.DoExtension {
    public static class DoExtensionMethod {
        public static void Do<T>(this T returnedValue, Func<object[], T> callback) {
            SubstituteContext.RegisterBehaviour(
                new DoMethodBehaviour<T>(callback));
        }
    }
}