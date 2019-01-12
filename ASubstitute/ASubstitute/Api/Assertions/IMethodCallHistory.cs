using System.Collections.Immutable;

namespace ASubstitute.Api.Assertions {
    public interface IMethodCallHistory {
        IImmutableList<IMethodCall> GetCalledMethods();
    }
}