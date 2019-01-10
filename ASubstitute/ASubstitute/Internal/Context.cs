using System.Reflection;
using System.Collections.Generic;
using System;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    class Context {
        private ProxyMethodCall _currentMethodCall;
        private MethodCallMatcherBuilder _methodCallMatcherBuilder;
        private MethodCallMatcher _methodCallMatcher;

        public Context() {
            Clear();
        }

        public void Clear() {
            _currentMethodCall = null;
            _methodCallMatcherBuilder = MethodCallMatcherBuilder.Create();
            _methodCallMatcher = null;
        }

        public void AddArgumentMatcher(IArgumentMatcher matcher) {
            _methodCallMatcherBuilder.AddExplicitMatcher(matcher);
        }

        public void SetCurrentMethodCall(ProxyMethodCall methodCall) {
            _currentMethodCall = methodCall;
            _methodCallMatcher = _methodCallMatcherBuilder
                .WithMethodCall(methodCall)
                .Build();

            _methodCallMatcherBuilder = MethodCallMatcherBuilder.Create();
        }

        public void RegisterBehaviour(IMethodBehaviour behaviour) {
            _methodCallMatcher.Verify();

            var proxy = _currentMethodCall.Proxy;

            // TODO: FindOrCreate ?
            MethodSetup existing = 
                proxy.FindCompatibleMethodSetup(_methodCallMatcher);

            if (existing == null) {
                existing = new MethodSetup(_methodCallMatcher);
                proxy.MethodSetups.Add(existing);
            }

            existing.AddBehaviour(behaviour);
        }
    }
}