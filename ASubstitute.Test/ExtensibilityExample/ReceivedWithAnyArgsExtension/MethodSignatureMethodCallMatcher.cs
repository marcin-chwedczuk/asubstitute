using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class MethodSignatureMethodCallMatcher : IMethodCallMatcher {
        private readonly IAssertionCall _assertionCall;

        public IMethod Method
            => _assertionCall.Method;

        public IImmutableList<IArgumentMatcher> ArgumentMatchers
            => _assertionCall.ArgumentMatchers;

        public MethodSignatureMethodCallMatcher(IAssertionCall original) {
            _assertionCall = original;
        }

        public bool MatchesCall(IMethodCall call) {
            // We ignore argument matchers

            return this.Method.HasSameSignatureAs(call.CalledMethod);
        }
    }
}