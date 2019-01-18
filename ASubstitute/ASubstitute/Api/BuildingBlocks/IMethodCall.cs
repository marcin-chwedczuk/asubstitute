using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethodCall
    {
        IMock Mock { get; }

        IMethod CalledMethod { get; }

        IImmutableList<object> PassedArguments { get; }
    }
}