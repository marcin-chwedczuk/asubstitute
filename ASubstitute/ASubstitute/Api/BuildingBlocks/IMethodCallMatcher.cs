using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallMatcher
    {
        // TODO: Group into IMethodCall AssertionCall
        IMethod Method { get; }

        IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        bool MatchesCall(IMethodCall call);
    }
}