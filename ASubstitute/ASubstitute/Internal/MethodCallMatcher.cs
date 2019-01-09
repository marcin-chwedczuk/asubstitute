using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    public class MethodCallMatcher {
        private readonly string _methodName;
        private readonly int _parametersCount;

        private readonly IImmutableList<IArgumentMatcher> _argumentMatchers;

        public MethodCallMatcher(
            string methodName, 
            int parametersCount, 
            IImmutableList<IArgumentMatcher> argumentMatchers)
        {
            _methodName = methodName;
            _parametersCount = parametersCount;
            _argumentMatchers = argumentMatchers;

            if (_parametersCount != _argumentMatchers.Count)
                throw new ArgumentException(
                    $"Expecting to get {_parametersCount} argument matchers, instead got {_argumentMatchers.Count}.");
        }

        public bool MatchesCall(ProxyMethod method, IImmutableList<TypedArgument> arguments) {
            if (string.CompareOrdinal(method.Name, _methodName) != 0) 
                return false;

            if (arguments.Count != _parametersCount)
                return false;

            for (int i = 0; i < arguments.Count; i++) {
                TypedArgument arg = arguments[i];
                IArgumentMatcher matcher = _argumentMatchers[i];

                // Call Matches<arg.Type>(arg.Value, matcher)
                var matchesMethod = typeof(MethodCallMatcher).GetMethod(nameof(Matches));
                MethodInfo generic = matchesMethod.MakeGenericMethod(arg.Type);
                var result = (bool) generic.Invoke(this, new object[] { arg.Value, matcher });

                if (!result) return false;
            }

            return true;
        }

        public void Verify() {
            var missingMatcherPlaceholder = _argumentMatchers
                .OfType<MissingArgumentMatcherPlaceholder>()
                .FirstOrDefault();

            if (missingMatcherPlaceholder != null) {
                throw new SubstituteException(missingMatcherPlaceholder.Message);
            }
        }

        public static bool Matches<T>(T argumentValue, IArgumentMatcher matcher) {
            if (matcher is IArgumentMatcher<T> typedMatcher) {
                return typedMatcher.Matches(argumentValue);
            }

            return false;
        }
        
        public bool IsCompatible(ProxyMethod method, IList<IArgumentMatcher> matchers) {
            if (string.CompareOrdinal(method.Name, _methodName) != 0) 
                return false;

            if (matchers.Count != _parametersCount)
                return false;

            for (int i = 0; i < matchers.Count; i++) {
                if (!matchers[i].Equals(_argumentMatchers[i]))
                    return false;
            }

            return true;
        }

        public bool IsCompatible(MethodCallMatcher other) {
            if (string.CompareOrdinal(this._methodName, other._methodName) != 0) 
                return false;

            if (this._parametersCount != other._parametersCount)
                return false;

            for (int i = 0; i < this._argumentMatchers.Count; i++) {
                var thisMatcher = this._argumentMatchers[i];
                var otherMatcher = other._argumentMatchers[i];

                if (thisMatcher.Equals(otherMatcher))
                    return false;
            }

            return true;
        }
    }
}