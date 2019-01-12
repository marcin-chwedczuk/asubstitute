using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    public class MethodSetup {
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

        public bool MatchesCall(ProxyMethod method, IImmutableList<TypedArgument> arguments) {
            return _methodCallMatcher.MatchesCall(method, arguments);
        }

        public object InvokeBehaviour(IImmutableList<TypedArgument> arguments) {
            IMethodBehaviour behaviour = _recordedBehaviours.Count > 1
                ? _recordedBehaviours.Dequeue()
                : _recordedBehaviours.Peek();

            return behaviour.Invoke(
                arguments.Select(x => x.Value).ToArray());
        }

        public bool IsCompatible(MethodCallMatcher matcher)
            => _methodCallMatcher.Equals(matcher);
    }

}