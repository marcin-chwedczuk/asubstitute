using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    public class AssertionCall : IAssertionCall {
        public IMethod Method { get; }

        public IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        public AssertionCall(
            IMethod method, 
            IImmutableList<IArgumentMatcher> argumentMatchers)
        {
            Method = method;
            ArgumentMatchers = argumentMatchers;    
        }

        public AssertionCall(MethodCallMatcher matcher)
            : this(matcher.Method, matcher.ArgumentMatchers) { }

        public IMethodCallMatcher ToMethodCallMatcher() {
            return new MethodCallMatcher(Method, ArgumentMatchers);
        }
    }
}