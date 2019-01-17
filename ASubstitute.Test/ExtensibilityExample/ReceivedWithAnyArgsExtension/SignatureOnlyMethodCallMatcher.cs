using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class SignatureOnlyMethodCallMatcher : IMethodCallMatcher {
        private readonly IMethod _method;

        public SignatureOnlyMethodCallMatcher(IMethod method) {
            _method = method;
        }

        public bool MatchesCall(IMethodCall call) {
            return this._method.HasSameSignatureAs(call.CalledMethod);
        }
    }
}