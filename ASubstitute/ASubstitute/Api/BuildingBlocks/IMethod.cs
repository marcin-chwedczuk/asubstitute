using System;
using System.Collections.Immutable;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethod {
        // TODO: Add proxy namespace Name

        string Name { get; }

        IImmutableList<Type> ParameterTypes { get; }

        // TODO: Extract to extension method
        bool HasSameSignatureAs(IMethod other);
    }
}