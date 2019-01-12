using System;
using System.Linq;
using System.Reflection;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class Proxy : DispatchProxy {
        private readonly MethodCallHistory _methodCallHistory = new MethodCallHistory();

        // TODO: Make private
        public IList<MethodSetup> MethodSetups { get; } = new List<MethodSetup>();

        public Type ProxiedType { get; private set; }
        
        private CallMode _callMode = CallMode.SetupReplay;
        private int _times = 0;

        internal void Init(Type proxiedType) {
            ProxiedType = proxiedType;
        }

        public void VerifyNextCallWasReceived(int times) {
            _callMode = CallMode.Verify;
            _times = times;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            var methodCall = ProxyMethodCall.From(this, targetMethod, args);
            
            // TODO: CQS broken?
            var methodCallMatcher = ThreadLocalContext.SetCurrentMethodCall(methodCall);

            if (_callMode == CallMode.SetupReplay) {
                _methodCallHistory.AddCall(methodCall);

                var recordedCall = FindFirstMatching(methodCall.CalledMethod, methodCall.PassedArguments);
                if (recordedCall != null) {
                    var result = recordedCall.InvokeBehaviour(methodCall.PassedArguments);

                    // TODO: is this if needed?
                    if (!object.ReferenceEquals(result, MethodSetup.NO_RESULT)) {
                        return result;
                    }
                }
            }
            else if(_callMode == CallMode.Verify) {
                int matchingCallsCount = _methodCallHistory.GetCalledMethods() 
                    .Where(call => methodCallMatcher.MatchesCall(call.CalledMethod, call.PassedArguments))
                    .Count();

                if (matchingCallsCount != _times)
                    throw new SubstituteException($"Expected: {_times} but got: {matchingCallsCount}");
            }
            else {
                throw new NotImplementedException();
            }

            return ReflectionUtils.CreateDefaultValue(targetMethod.ReturnType);
        }

        internal void MarkMethodCallAsSetup(ProxyMethodCall call) {
            _methodCallHistory.RemoveCall(call);
        }

        public MethodSetup FindFirstMatching(ProxyMethod method, IImmutableList<TypedArgument> typedArgs) {
            return MethodSetups
                .FirstOrDefault(x => x.MatchesCall(method, typedArgs));
        }

        public MethodSetup FindCompatibleMethodSetup(MethodCallMatcher matcher) {
            return MethodSetups
                .SingleOrDefault(x => x.IsCompatible(matcher));
        }
    }
    public enum CallMode {
        SetupReplay,
        Verify
    }
}