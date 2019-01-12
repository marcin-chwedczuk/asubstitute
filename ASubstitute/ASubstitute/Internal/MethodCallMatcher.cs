using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;
using ASubstitute.Api;

namespace ASubstitute.Internal {
    public class MethodCallMatcher {
        private readonly string _methodName;

        private readonly IImmutableList<IArgumentMatcher> _argumentMatchers;

        public MethodCallMatcher(
            string methodName, 
            IImmutableList<IArgumentMatcher> argumentMatchers)
        {
            _methodName = methodName;
            _argumentMatchers = argumentMatchers;
        }

        public bool MatchesCall(ProxyMethod method, IImmutableList<TypedArgument> arguments) {
            if (!string.Equals(_methodName, method.Name, StringComparison.Ordinal))
                return false;

            if (_argumentMatchers.Count != arguments.Count)
                return false;

            for (int i = 0; i < _argumentMatchers.Count; i++) {
                IArgumentMatcher matcher = _argumentMatchers[i];
                TypedArgument argument = arguments[i];

                if (!Matches(argument, matcher)) {
                    return false;
                }
            }

            return true;
        }

        private static bool Matches(TypedArgument argument, IArgumentMatcher matcher) {
            // Calls Matches<arg.Type>(arg.Value, matcher)
            var genericMethod = typeof(MethodCallMatcher)
                .GetMethod(nameof(MatchesImpl), BindingFlags.NonPublic | BindingFlags.Static);

            var concreteMethod = genericMethod.MakeGenericMethod(argument.Type);

            var args = new[] { argument.Value, matcher };
            return (bool) concreteMethod.Invoke(null, args);
        }

        private static bool MatchesImpl<T>(T argumentValue, IArgumentMatcher matcher) {
            if (matcher is IArgumentMatcher<T> typedMatcher) {
                return typedMatcher.Matches(argumentValue);
            }

            return false;
        }
 
        public void VerifyIsComplete() {
            // TODO: More OO way like matchers foreach [ it | it.VerifyValid() ]
            var missingMatcherPlaceholder = _argumentMatchers
                .OfType<MissingArgumentMatcherPlaceholder>()
                .FirstOrDefault();

            if (missingMatcherPlaceholder != null) {
                throw new SubstituteException(missingMatcherPlaceholder.InvalidMatcherUsageMessage);
            }
        }
   }
}