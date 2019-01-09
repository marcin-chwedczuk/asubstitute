using ASubstitute.Internal;

namespace ASubstitute {
    public static class Actions {
        public static T Returns<T>(this T obj, T valueToReturn) {
            // TODO: Add public api for custom matchers (ThredLocalContext)
            ThreadLocalContext.RegisterBehaviour(
                new ReturnValueRecordedBehaviour(valueToReturn));

            return default(T);
        }
    }
}