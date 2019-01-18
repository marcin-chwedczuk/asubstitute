using System.Linq;
using System.Text;
using System.Collections.Immutable;
using ASubstitute.Internal;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IAssertionCall {
        IMethod Method { get; }

        IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }
    }

    public static class IAssertionCallExtensionMethods {
        public static string Describe(this IAssertionCall @this) {
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

        public static IMethodCallMatcher ToMethodCallMatcher(this IAssertionCall @this) {
            return new MethodCallMatcher(@this.Method, @this.ArgumentMatchers);
        }
    }
}
