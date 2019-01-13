using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallMatcher
    {
        bool MatchesCall(IMethod method, IImmutableList<ITypedArgument> arguments);
    }
}