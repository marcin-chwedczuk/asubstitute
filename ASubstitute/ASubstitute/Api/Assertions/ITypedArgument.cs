using System;

namespace ASubstitute.Api.Assertions {
    public interface ITypedArgument
    {
        Type Type { get; }
        object Value { get; }
    }
}