using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;
using System.Diagnostics;

namespace ASubstitute.Internal {
    public class MethodCallMatcher : IMethodCallMatcher {
        public IMethod Method { get; }

        public IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        public MethodCallMatcher(
            IMethod method, 
            IImmutableList<IArgumentMatcher> argumentMatchers)
        {
            if (method.ParameterTypes.Count != argumentMatchers.Count)
                throw new ArgumentException(
                    "Number of argument matchers passed to constructor differs " +
                    "from number of method parameters.");

            Method = method;
            ArgumentMatchers = argumentMatchers;
        }

        public bool MatchesCall(IMethod calledMethod, IImmutableList<ITypedArgument> arguments) {
            if (!Method.HasSameSignatureAs(calledMethod))
                return false;

            Debug.Assert(ArgumentMatchers.Count == arguments.Count);

            for (int i = 0; i < ArgumentMatchers.Count; i++) {
                IArgumentMatcher matcher = ArgumentMatchers[i];
                ITypedArgument argument = arguments[i];

                if (!Matches(argument, matcher)) {
                    return false;
                }
            }

            return true;
        }

        private static bool Matches(ITypedArgument argument, IArgumentMatcher matcher) {
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
            var missingMatcherPlaceholder = ArgumentMatchers
                .OfType<MissingArgumentMatcherPlaceholder>()
                .FirstOrDefault();

            if (missingMatcherPlaceholder != null) {
                throw new SubstituteException(missingMatcherPlaceholder.InvalidMatcherUsageMessage);
            }
        }
    }
}