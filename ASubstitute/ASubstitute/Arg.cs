using System;
using ASubstitute.Api;
using ASubstitute.Buildin.ArgumentMatchers;

namespace ASubstitute {
    public static class Arg {
        public static T Any<T>() {
            SubstituteContext.AddArgumentMatcher(
                new AnyArgumentMatcher<T>());

            return default(T);
        }

        public static T Is<T>(T value) {
            SubstituteContext.AddArgumentMatcher(
                new IsArgumentMatcher<T>(value));

            return default(T);
        }

        public static T Is<T>(Predicate<T> predicate) {
            SubstituteContext.AddArgumentMatcher(
                new PredicateArgumentMatcher<T>(predicate));

            return default(T);
        }
    }
}