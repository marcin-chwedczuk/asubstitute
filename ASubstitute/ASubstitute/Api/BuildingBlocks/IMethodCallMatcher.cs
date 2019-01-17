using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallMatcher
    {
        bool MatchesCall(IMethodCall call);
    }
}