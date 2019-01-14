using System;
using System.Linq;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Internal;

namespace ASubstitute.Buildin.Assertions {
    class MethodCalledNTimesAssertion : IMethodCallHistoryAssertion {
        private readonly int _times;

        public MethodCalledNTimesAssertion(int times) {
            if (times < 0) throw new ArgumentException($"{nameof(times)} cannot be negative.");

            _times = times;
        }

        public void Check(IMethodCallMatcher assertionCall, IMethodCallHistory methodCallHistory) {
            int matchingCallsCount = methodCallHistory.GetCalledMethods() 
                .Where(call => assertionCall.MatchesCall(call))
                .Count();

            if (matchingCallsCount != _times)
                throw new SubstituteException($"Expected: {_times} but got: {matchingCallsCount}");
        }
    }
}