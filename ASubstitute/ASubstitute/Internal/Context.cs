using System.Reflection;
using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace ASubstitute.Internal {
    class Context {
        private ProxyMethodCall _currentMethodCall;
        private MethodCallMatcherBuilder _methodCallMatcherBuilder;
        private MethodCallMatcher _methodCallMatcher;

        private IMethodCallHistoryAssertion _activeAssertion;

        public Context() {
            Clear();
        }

        public void Clear() {
            _currentMethodCall = null;
            _methodCallMatcherBuilder = MethodCallMatcherBuilder.Create();
            _methodCallMatcher = null;
            _activeAssertion = null;
        }

        public void AddArgumentMatcher(IArgumentMatcher matcher) {
            _methodCallMatcherBuilder.AddExplicitMatcher(matcher);
        }

        public MethodCallMatcher SetCurrentMethodCall(ProxyMethodCall methodCall) {
            _currentMethodCall = methodCall;
            _methodCallMatcher = _methodCallMatcherBuilder
                .WithMethodCall(methodCall)
                .Build();

            _methodCallMatcherBuilder = MethodCallMatcherBuilder.Create();
            return _methodCallMatcher;
        }

        public void RegisterBehaviour(IMethodBehaviour behaviour) {
            _methodCallMatcher.VerifyIsComplete();

            var proxy = _currentMethodCall.Proxy;

            // TODO: FindOrCreate ?
            MethodSetup existing = 
                proxy.FindCompatibleMethodSetup(_methodCallMatcher);

            if (existing == null) {
                existing = new MethodSetup(_methodCallMatcher);
                proxy.MethodSetups.Add(existing);
            }

            existing.AddBehaviour(behaviour);
            proxy.MarkMethodCallAsSetup(_currentMethodCall);
        }

        public void RegisterAssertion(IMethodCallHistoryAssertion assertion) {
            Debug.Assert(_activeAssertion == null);
            _activeAssertion = assertion;
        }

        public IMethodCallHistoryAssertion ConsumeAssertion() {
            var tmp = _activeAssertion;
            _activeAssertion = null;
            return tmp;
        }
    }
}