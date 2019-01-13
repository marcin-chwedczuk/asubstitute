using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCall
    {
        IMethod CalledMethod { get; }

        IImmutableList<ITypedArgument> PassedArguments { get; }
    }
}