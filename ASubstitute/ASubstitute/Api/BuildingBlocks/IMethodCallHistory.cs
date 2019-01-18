using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    // TODO: Rename to MockCallHistory
    public interface IMethodCallHistory {
        IImmutableList<IMethodCall> GetCalledMethods();
    }
}