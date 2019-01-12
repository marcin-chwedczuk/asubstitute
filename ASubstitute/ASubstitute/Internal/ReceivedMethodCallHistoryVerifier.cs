using System;
using System.Linq;

namespace ASubstitute.Internal {
    class MethodCalledNTimesAssertion : IMethodCallHistoryAssertion {
        private readonly int _times;

        public MethodCalledNTimesAssertion(int times) {
            if (times < 0) throw new ArgumentException($"{nameof(times)} cannot be negative.");

            _times = times;
        }

        public void Check(MethodCallMatcher assertionCall, MethodCallHistory methodCallHistory) {
            int matchingCallsCount = methodCallHistory.GetCalledMethods() 
                .Where(call => assertionCall.MatchesCall(call.CalledMethod, call.PassedArguments))
                .Count();

            if (matchingCallsCount != _times)
                throw new SubstituteException($"Expected: {_times} but got: {matchingCallsCount}");
        }
    }
}