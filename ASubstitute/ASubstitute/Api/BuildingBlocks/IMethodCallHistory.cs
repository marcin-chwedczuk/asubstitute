using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCallHistory {
        IImmutableList<IMethodCall> GetCalledMethods();
    }
}