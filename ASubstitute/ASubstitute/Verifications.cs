using ASubstitute.Internal;

namespace ASubstitute {
    public static class Verifications {
        public static T Received<T>(this T mock, int times) {
            (mock as Proxy).VerifyNextCallWasReceived(times);
            return mock;
        }

        public static T DidNotReceive<T>(this T mock)
            => Received(mock, times: 0);
    }
}