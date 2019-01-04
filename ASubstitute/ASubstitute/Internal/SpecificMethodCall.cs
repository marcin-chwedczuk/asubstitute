using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class SpecificMethodCall {
        public static readonly object NO_RESULT = new object();

        public readonly MethodCallMatcher _methodCallMatcher;
        public readonly Queue<IRecordedBehaviour> _recordedBehaviours
            = new Queue<IRecordedBehaviour>();

        public SpecificMethodCall(MethodCallMatcher methodCallMatcher) {
            _methodCallMatcher = methodCallMatcher;
        }

        public void AddBehaviour(IRecordedBehaviour behaviour) {
            _recordedBehaviours.Enqueue(behaviour);
        }

        public bool MatchesCall(MethodInfo method, IList<TypedArgument> arguments) {
            return _methodCallMatcher.MatchesCall(method, arguments);
        }

        public object InvokeBehaviour(IList<TypedArgument> arguments) {
            IRecordedBehaviour behaviour = _recordedBehaviours.Count > 1
                ? _recordedBehaviours.Dequeue()
                : _recordedBehaviours.Peek();

            return behaviour.Invoke(
                arguments.Select(x => x.Value).ToArray());
        }

        internal bool IsCompatible(MethodInfo method, IList<IArgumentMatcher> matchers) {
            return _methodCallMatcher.IsCompatible(method, matchers);
        }
    }

}