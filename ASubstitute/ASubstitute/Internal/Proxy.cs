using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class Proxy : DispatchProxy {
        public IList<SpecificMethodCall> MethodCalls { get; } = new List<SpecificMethodCall>();

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            var typedArgs = targetMethod.GetParameters()
                .Select((param, index) => new TypedArgument(param.ParameterType, args[index]))
                .ToArray();
                
            ThreadLocalContext.RecordInvocation(this, targetMethod, typedArgs);

            var methodCall = FindFirstMatching(targetMethod, typedArgs);
            if (methodCall != null) {
                var result = methodCall.InvokeBehaviour(typedArgs);

                if (!object.ReferenceEquals(result, SpecificMethodCall.NO_RESULT)) {
                    return result;
                }
            }

            return ReflectionUtils.CreateDefaultValue(targetMethod.ReturnType);
        }

        public SpecificMethodCall FindFirstMatching(MethodInfo method, IList<TypedArgument> typedArgs) {
            return MethodCalls
                .FirstOrDefault(x => x.MatchesCall(method, typedArgs));
        }

        public SpecificMethodCall FindCompatibleMethodCall(MethodInfo method, IList<IArgumentMatcher> matchers) {
            return MethodCalls
                .FirstOrDefault(x => x.IsCompatible(method, matchers));
        }
    }

}