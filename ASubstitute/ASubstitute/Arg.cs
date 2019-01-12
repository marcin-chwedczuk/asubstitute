using System;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Arg {
        public static T Any<T>() {
            ThreadLocalContext.AddArgumentMatcher(
                new MatchAnyArgumentMatcher<T>());

            return default(T);
        }

        public static T Is<T>(T value) {
            ThreadLocalContext.AddArgumentMatcher(
                new MatchEqualToArgumentMatcher<T>(value));

            return default(T);
        }

        public static T Is<T>(Predicate<T> predicate) {
            ThreadLocalContext.AddArgumentMatcher(
                new FulfillsPredicateArgumentMatcher<T>(predicate));

            return default(T);
        }
    }
}