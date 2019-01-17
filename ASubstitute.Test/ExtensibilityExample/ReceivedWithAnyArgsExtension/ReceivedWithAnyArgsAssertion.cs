using System.Linq;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Internal;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class ReceivedWithAnyArgsAssertion : IMethodCallHistoryAssertion {
        public void Check(IAssertionCall assertionCall, IMethodCallHistory methodCallHistory) {
            var matcher = new SignatureOnlyMethodCallMatcher(assertionCall.Method);
            
            int matchingCallsCount = methodCallHistory
                .GetCalledMethods() 
                .Where(matcher.MatchesCall)
                .Count();

            // TODO: Add proxy name and full method signature to error message.
            // Put signature at the end (to not clutter error message).
            if (matchingCallsCount == 0)
                throw new SubstituteException(
                    $"Expecting to receive at least a single call to method {assertionCall.Describe()} " +
                    "but none was received.");

            // TODO: Describe() will fail with missing arg matcher (AddTwoIntegers(0, 0)).
            // Quickfix: create assertion call from method, using anymatcher for each
            // argument...
        }
    }
}