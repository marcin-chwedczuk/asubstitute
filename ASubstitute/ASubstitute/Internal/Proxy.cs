﻿using System;
using System.Linq;
using System.Reflection;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    // Must be public due to DispatchProxy requirements.
    public class Proxy : DispatchProxy {
        private readonly MethodCallHistory _methodCallHistory = new MethodCallHistory();

        // TODO: Make private
        internal IList<MethodSetup> MethodSetups { get; } = new List<MethodSetup>();

        public Type ProxiedType { get; private set; }
        
        internal void Init(Type proxiedType) {
            ProxiedType = proxiedType;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            var methodCall = ProxyMethodCall.From(this, targetMethod, args);
            
            // TODO: CQS broken?
            var methodCallMatcher = ThreadLocalContext.SetCurrentMethodCall(methodCall);
            var activeAssertion = ThreadLocalContext.ConsumeAssertion();

            if (activeAssertion != null) {
                activeAssertion.Check(methodCallMatcher, _methodCallHistory);
            }
            else {
                _methodCallHistory.AddCall(methodCall);

                var recordedCall = MethodSetups.FirstOrDefault(m => m.MatchesCall(methodCall));
                if (recordedCall != null) {
                    var result = recordedCall.InvokeBehaviour(methodCall.PassedArguments);

                    // TODO: is this if needed?
                    if (!object.ReferenceEquals(result, MethodSetup.NO_RESULT)) {
                        return result;
                    }
                }
            }

            return ReflectionUtils.CreateDefaultValue(targetMethod.ReturnType);
        }

        internal void MarkMethodCallAsSetup(ProxyMethodCall call) {
            _methodCallHistory.RemoveCall(call);
        }

        internal MethodSetup FindCompatibleMethodSetup(MethodCallMatcher matcher) {
            return MethodSetups
                .SingleOrDefault(x => x.IsCompatible(matcher));
        }
    }
}