using System;
using ASubstitute.Buildin.ArgumentMatchers;
using ASubstitute.Buildin.Assertions;
using ASubstitute.Buildin.Behaviours;

namespace ASubstitute.Api {
    public static class SubstituteBuildin {
        public static IArgumentMatcher AnyArgumentMatcher<T>()
            => new AnyArgumentMatcher<T>();

        public static IArgumentMatcher IsArgumentMatcher<T>(T value)
            => new IsArgumentMatcher<T>(value);

        public static IArgumentMatcher PredicateArgumentMatcher<T>(Predicate<T> predicate)
            => new PredicateArgumentMatcher<T>(predicate);

        public static IMethodBehaviour Returns<T>(T value)
            => new ReturnsMethodBehaviour(value);

        public static IMethodBehaviour Throws(Exception ex)
            => new ThrowsMethodBehaviour(ex);

        public static IMethodCallHistoryAssertion ReceivedAssertion(int times)
            => new ReceivedAssertion(times);
    }
}