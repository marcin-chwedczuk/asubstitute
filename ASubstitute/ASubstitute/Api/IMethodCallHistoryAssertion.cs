using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Api {
    public interface IMethodCallHistoryAssertion {
        void Check(IAssertionCall assertionCall, IMethodCallHistory methodCallHistory);
    }
}