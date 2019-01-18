using System;

namespace ASubstitute.Api {
    public interface IMock {
        string Name { get; }
        Type MockedType { get; }
    }
}