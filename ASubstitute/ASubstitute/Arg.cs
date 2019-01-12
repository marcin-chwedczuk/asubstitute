using System;
using ASubstitute.Buildin.ArgumentMatchers;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Arg {
        public static T Any<T>() {
            ThreadLocalContext.AddArgumentMatcher(
                new AnyArgumentMatcher<T>());

            return default(T);
        }

        public static T Is<T>(T value) {
            ThreadLocalContext.AddArgumentMatcher(
                new EqualToArgumentMatcher<T>(value));

            return default(T);
        }

        public static T Is<T>(Predicate<T> predicate) {
            ThreadLocalContext.AddArgumentMatcher(
                new PredicateArgumentMatcher<T>(predicate));

            return default(T);
        }
    }
}