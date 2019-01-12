using System.Collections.Immutable;

namespace ASubstitute.Api.Assertions {
    public interface IMethodCallMatcher
    {
        bool MatchesCall(IMethod method, IImmutableList<ITypedArgument> arguments);
    }
}