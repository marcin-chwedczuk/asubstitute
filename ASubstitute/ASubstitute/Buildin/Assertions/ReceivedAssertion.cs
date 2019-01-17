using System;
using System.Linq;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Internal;

namespace ASubstitute.Buildin.Assertions {
    class ReceivedAssertion: IMethodCallHistoryAssertion {
        private readonly int _times;

        public ReceivedAssertion(int times) {
            if (times < 0) throw new ArgumentException($"{nameof(times)} cannot be negative.");

            _times = times;
        }

        public void Check(IAssertionCall assertionCall, IMethodCallHistory methodCallHistory) {
            var matcher = assertionCall.ToMethodCallMatcher();

            int matchingCallsCount = methodCallHistory
                .GetCalledMethods() 
                .Where(matcher.MatchesCall)
                .Count();

            if (matchingCallsCount != _times)
                throw new SubstituteException(
                    $"Expected *{_times}* call(s) matching assertion:\n" +
                    $"\t{assertionCall.Describe()}\n" +
                    $"but got *{matchingCallsCount}* matching calls.");
        }
    }
}