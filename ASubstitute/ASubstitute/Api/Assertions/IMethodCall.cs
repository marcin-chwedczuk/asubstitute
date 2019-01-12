using System.Collections.Immutable;

namespace ASubstitute.Api.Assertions {
    public interface IMethodCall
    {
        IMethod CalledMethod { get; }

        IImmutableList<ITypedArgument> PassedArguments { get; }
    }
}