using System.Linq;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Internal;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class ReceivedWithAnyArgsAssertion : IMethodCallHistoryAssertion {
        public void Check(IMethodCallMatcher assertionCall, IMethodCallHistory methodCallHistory) {
            var matcher = new MethodSignatureMethodCallMatcher(assertionCall);
            
            int matchingCallsCount = methodCallHistory.GetCalledMethods() 
                .Where(call => matcher.MatchesCall(call.CalledMethod, call.PassedArguments))
                .Count();

            // TODO: Add proxy name and full method signature to error message.
            // Put signature at the end (to not clutter error message).
            if (matchingCallsCount == 0)
                throw new SubstituteException(
                    $"Expecting to receive at least a single call to method {matcher.Method.Name} " +
                    "but none was received.");
 
        }
    }
}