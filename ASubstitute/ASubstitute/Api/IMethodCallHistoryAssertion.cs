using ASubstitute.Api.Assertions;

namespace ASubstitute.Api {
    interface IMethodCallHistoryAssertion {
        void Check(IMethodCallMatcher assertionCall, IMethodCallHistory methodCallHistory);
    }
}