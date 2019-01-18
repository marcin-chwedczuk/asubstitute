using System.Linq;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Internal;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class ReceivedWithAnyArgsAssertion : IMethodCallHistoryAssertion {
        public void Check(IAssertionCall assertionCall, IMethodCallHistory methodCallHistory) {
            var anyArgsAssertionCall = new AnyArgumentsAssertionCall(assertionCall);
            var matcher = anyArgsAssertionCall.ToMethodCallMatcher();

            int matchingCallsCount = methodCallHistory
                .GetCalledMethods() 
                .Where(matcher.MatchesCall)
                .Count();

            // TODO: Add proxy name and full method signature to error message.
            // Put signature at the end (to not clutter error message).
            if (matchingCallsCount == 0)
                throw new SubstituteException(
                    $"Expecting to receive at least a single call to method " +
                    $"{anyArgsAssertionCall.Describe()} " +
                    $"but none was received.");
        }
    }
}