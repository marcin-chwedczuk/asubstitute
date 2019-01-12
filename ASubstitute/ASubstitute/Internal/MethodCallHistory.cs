using System.Collections.Generic;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    class MethodCallHistory {
        private readonly List<ProxyMethodCall> _receivedCalls = new List<ProxyMethodCall>();

        public void AddCall(ProxyMethodCall methodCall)
            => _receivedCalls.Add(methodCall);

        public void RemoveCall(ProxyMethodCall methodCall)
            => _receivedCalls.Remove(methodCall);

        public IImmutableList<ProxyMethodCall> GetCalledMethods()
            => _receivedCalls.ToImmutableList();
    }
}