namespace ASubstitute.Internal {
    interface IMethodCallHistoryAssertion {
        void Check(MethodCallMatcher assertionCall, MethodCallHistory methodCallHistory);
    }
}