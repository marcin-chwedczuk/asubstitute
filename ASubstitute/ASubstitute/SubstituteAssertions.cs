using ASubstitute.Api;
using ASubstitute.Buildin.Assertions;

namespace ASubstitute {
    public static class SubstituteAssertions {
        public static T Received<T>(this T mock, int times) {
            SubstituteContext.RegisterAssertion(
                new ReceivedAssertion(times));

            return mock;
        }

        public static T DidNotReceive<T>(this T mock)
            => Received(mock, times: 0);
    }
}