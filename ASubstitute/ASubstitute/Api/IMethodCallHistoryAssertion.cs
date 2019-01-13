using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Api {
    interface IMethodCallHistoryAssertion {
        void Check(IMethodCallMatcher assertionCall, IMethodCallHistory methodCallHistory);
    }
}