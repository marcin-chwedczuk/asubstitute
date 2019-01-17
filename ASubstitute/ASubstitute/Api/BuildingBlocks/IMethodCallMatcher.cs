using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallMatcher
    {
        // TODO: Group into IMethodCall AssertionCall
        IMethod Method { get; }

        IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        bool MatchesCall(IMethodCall call);
    }
}