using System;

namespace ASubstitute.Api.BuildingBlocks {
    public interface ITypedArgument
    {
        Type Type { get; }
        object Value { get; }
    }
}