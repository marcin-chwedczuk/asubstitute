using System;
using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethod {
        string Name { get; }

        IImmutableList<Type> ParameterTypes { get; }

        bool HasSameSignatureAs(IMethod other);
    }
}