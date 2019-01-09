using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ASubstitute.Internal {
    class MethodCallMatcherBuilder {
        private ProxyMethodCall _methodCall;
        private readonly List<IArgumentMatcher> _explicitMatchers 
            = new List<IArgumentMatcher>();

        private MethodCallMatcherBuilder() { }

        public MethodCallMatcher Build() {
            CreateAllArgumentsMatchers();

            return new MethodCallMatcher(
                _methodCall.CalledMethod.Name,
                _methodCall.CalledMethod.GetParametersCount(),
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

            foreach(var (arg, index) in _methodCall.PassedArguments.ToValueIndexPairs()) {
                IArgumentMatcher matcher = arg.HasDefaultValue
                    ? explicitMatchers.DequeueOrElse(
                            () => CreateMissingExplicitMatcherPlaceholder(_methodCall, index))
                    : CreateMatcherFromArgumentValue(arg);
                
                allArgumentsMatchers.Add(matcher);
            }

            return allArgumentsMatchers.ToImmutable();
        }

        private static IArgumentMatcher CreateMatcherFromArgumentValue(TypedArgument arg) {
            var type = typeof(MatchEqualToArgumentMatcher<>).MakeGenericType(arg.Type);
            return (IArgumentMatcher) Activator.CreateInstance(type, arg.Value);
        }

        private IArgumentMatcher CreateMissingExplicitMatcherPlaceholder(
            ProxyMethodCall methodCall, 
            int argIndex) 
        {
            var message = 
                $"Invalid usage of argument matchers detected in call to " +
                $"{methodCall.Proxy.ProxiedType.Name}.{methodCall.CalledMethod.Name} method. " +
                $"Missing argument matcher for parameter on position {argIndex}." +
                $"Please remember that all default values (null, 0, false, default) " +
                $"must be replaced by argument matchers in *setup* calls.";

            return new MissingArgumentMatcherPlaceholder(message);
        }

        public static MethodCallMatcherBuilder Create()
            => new MethodCallMatcherBuilder();
    }
}