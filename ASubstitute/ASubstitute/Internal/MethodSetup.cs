using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    class MethodSetup {
        public static readonly object NO_RESULT = new object();

        public readonly MethodCallMatcher _methodCallMatcher;
        public readonly Queue<IMethodBehaviour> _recordedBehaviours
            = new Queue<IMethodBehaviour>();

        public MethodSetup(MethodCallMatcher methodCallMatcher) {
            _methodCallMatcher = methodCallMatcher;
        }

        public void AddBehaviour(IMethodBehaviour behaviour) {
            _recordedBehaviours.Enqueue(behaviour);
        }

        public bool MatchesCall(ProxyMethodCall call) {
            return _methodCallMatcher.MatchesCall(call);
        }

        public object InvokeBehaviour(IImmutableList<object> arguments) {
            IMethodBehaviour behaviour = _recordedBehaviours.Count > 1
                ? _recordedBehaviours.Dequeue()
                : _recordedBehaviours.Peek();

            return behaviour.Invoke(arguments.ToArray());
        }

        public bool IsCompatible(MethodCallMatcher matcher)
            => _methodCallMatcher.Equals(matcher);
    }

}