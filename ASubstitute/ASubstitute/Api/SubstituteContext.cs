using ASubstitute;

namespace ASubstitute.Api {
    public static class SubstituteContext {
        public static void AddArgumentMatcher(IArgumentMatcher matcher)
            => Internal.ThreadLocalContext.AddArgumentMatcher(matcher);

        public static void RegisterBehaviour(IMethodBehaviour behaviour) 
            => Internal.ThreadLocalContext.RegisterBehaviour(behaviour);

        internal static void RegisterAssertion(IMethodCallHistoryAssertion assertion)
            => Internal.ThreadLocalContext.RegisterAssertion(assertion);
    }
}