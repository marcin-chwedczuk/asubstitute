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

    public static class IMethodCallMatcherExtensionMethods {
        public static string Describe(this IMethodCallMatcher @this) {
            var matchersDescriptions = @this
                .ArgumentMatchers
                .Select(m => m.Describe());

            return new StringBuilder()
                .Append(@this.Method.ProxyName)
                .Append('.')
                .Append(@this.Method.Name)
                .Append('(')
                .Append(string.Join(", ", matchersDescriptions))
                .Append(')')
                .ToString();
        }
    }
}