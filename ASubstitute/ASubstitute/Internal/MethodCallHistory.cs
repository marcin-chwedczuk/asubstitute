using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    class MethodCallHistory : IMethodCallHistory {
        private readonly List<IMethodCall> _receivedCalls = new List<IMethodCall>();

        public void AddCall(ProxyMethodCall methodCall)
            => _receivedCalls.Add(methodCall);

        public void RemoveCall(ProxyMethodCall methodCall)
            => _receivedCalls.Remove(methodCall);

        public IImmutableList<IMethodCall> GetCalledMethods()
            => _receivedCalls.ToImmutableList();
    }
}