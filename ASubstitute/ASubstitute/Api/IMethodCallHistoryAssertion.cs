using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Api {
    interface IMethodCallHistoryAssertion {
        void Check(IAssertionCall assertionCall, IMethodCallHistory methodCallHistory);
    }
}