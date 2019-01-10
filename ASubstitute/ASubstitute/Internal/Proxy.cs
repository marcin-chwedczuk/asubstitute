using System;
using System.Linq;
using System.Reflection;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class Proxy : DispatchProxy {
        public IList<MethodSetup> MethodSetups { get; } = new List<MethodSetup>();

        public Type ProxiedType { get; private set; }

        internal void Init(Type proxiedType) {
            ProxiedType = proxiedType;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args) {

            var methodCall = ProxyMethodCall.From(this, targetMethod, args);

            ThreadLocalContext.SetCurrentMethodCall(methodCall);

            var recordedCall = FindFirstMatching(methodCall.CalledMethod, methodCall.PassedArguments);
            if (recordedCall != null) {
                var result = recordedCall.InvokeBehaviour(methodCall.PassedArguments);

                // TODO: is this if needed?
                if (!object.ReferenceEquals(result, MethodSetup.NO_RESULT)) {
                    return result;
                }
            }

            return ReflectionUtils.CreateDefaultValue(targetMethod.ReturnType);
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

}