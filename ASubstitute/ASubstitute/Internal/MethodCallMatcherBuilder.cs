using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Buildin.ArgumentMatchers;

namespace ASubstitute.Internal {
    class MethodCallMatcherBuilder {
        private ProxyMethodCall _methodCall;
        private readonly List<IArgumentMatcher> _explicitMatchers 
            = new List<IArgumentMatcher>();

        private MethodCallMatcherBuilder() { }

        public MethodCallMatcher Build() {
            CreateAllArgumentsMatchers();

            return new MethodCallMatcher(
                _methodCall.CalledMethod,
                CreateAllArgumentsMatchers());
        }

        public MethodCallMatcherBuilder WithMethodCall(ProxyMethodCall methodCall) {
            _methodCall = methodCall;
            return this;
        }

        public void AddExplicitMatcher(IArgumentMatcher matcher)
            => _explicitMatchers.Add(matcher);

        private ImmutableList<IArgumentMatcher> CreateAllArgumentsMatchers() {
            // The purpose of this method is to add argument matchers
            // for arguments that were specified by passing non-default values.
            // In other words we transform:
            //      _component.MethodCall(1, Arg.Any<int>(), 5)
            // into:
            //      _component.MethodCall(Arg.Is(1), Arg.Any<int>(), Arg.Is(5))
            //
            // Missing argument matchers are replaced by MissingArgumentMatcherPlaceholder instances.
            // Also notice that missing argument matchers will be used quite often,
            // every *non-setup* method call with a default value will trigger
            // creation of a missing argument matcher.

            var allArgumentsMatchers = ImmutableList.CreateBuilder<IArgumentMatcher>();
            var explicitMatchers = new Queue<IArgumentMatcher>(_explicitMatchers);

            for (int i = 0; i < _methodCall.PassedArguments.Count; i++) {
                Type parameterType = _methodCall.CalledMethod.ParameterTypes[i];
                object argumentValue = _methodCall.PassedArguments[i];

                // TODO: To extension method on type, isDefaultValue
                IArgumentMatcher matcher = ReflectionUtils.IsDefaultValue(parameterType, argumentValue)
                    ? explicitMatchers.DequeueOrElse(() => 
                            new MissingArgumentMatcherPlaceholder(_methodCall, i))
                    : CreateMatcherFromArgumentValue(parameterType, argumentValue);
                
                allArgumentsMatchers.Add(matcher);
            }

            return allArgumentsMatchers.ToImmutable();
        }

        private static IArgumentMatcher CreateMatcherFromArgumentValue(Type parameterType, object argumentValue) {
            var type = typeof(IsArgumentMatcher<>).MakeGenericType(parameterType);
            return (IArgumentMatcher) Activator.CreateInstance(type, argumentValue);
        }

        public static MethodCallMatcherBuilder Create()
            => new MethodCallMatcherBuilder();
    }
}