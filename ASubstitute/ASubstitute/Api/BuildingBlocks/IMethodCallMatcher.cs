using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallMatcher
    {
        IMethod Method { get; }

        IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        bool MatchesCall(IMethod method, IImmutableList<ITypedArgument> arguments);
    }
}