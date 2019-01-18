using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    public class AssertionCall : IAssertionCall {
        public IMock Mock { get; }

        public IMethod Method { get; }

        public IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        public AssertionCall(
            IMock mock,
            IMethod method, 
            IImmutableList<IArgumentMatcher> argumentMatchers)
        {
            Mock = mock;
            Method = method;
            ArgumentMatchers = argumentMatchers;    
        }

        public IMethodCallMatcher ToMethodCallMatcher() {
            return new MethodCallMatcher(Method, ArgumentMatchers);
        }
    }
}