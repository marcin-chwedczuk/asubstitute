using ASubstitute.Api;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public static class ReceivedWithAnyArgsExtensionMethod {
        public static T ReceivedWithAnyArgs<T>(this T proxy) {
            SubstituteContext.RegisterAssertion(
                new ReceivedWithAnyArgsAssertion());

            return proxy;
        }
    }
}