using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class MethodSignatureMethodCallMatcher : IMethodCallMatcher {
        private readonly IMethodCallMatcher _original;

        public IMethod Method
            => _original.Method;

        public IImmutableList<IArgumentMatcher> ArgumentMatchers
            => _original.ArgumentMatchers;

        public MethodSignatureMethodCallMatcher(IMethodCallMatcher original) {
            _original = original;
        }

        public bool MatchesCall(IMethod method, IImmutableList<ITypedArgument> arguments) {
            // We ignore argument matchers

            return this.Method.HasSameSignatureAs(method);
        }
    }
}