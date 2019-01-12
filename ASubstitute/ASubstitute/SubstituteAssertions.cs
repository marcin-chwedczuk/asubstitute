using ASubstitute.Internal;

namespace ASubstitute {
    public static class SubstituteAssertions {
        public static T Received<T>(this T mock, int times) {
            ThreadLocalContext.RegisterAssertion(
                new MethodCalledNTimesAssertion(times));

            return mock;
        }

        public static T DidNotReceive<T>(this T mock)
            => Received(mock, times: 0);
    }
}