using System;
using ASubstitute.Api;

namespace ASubstitute.Internal {
    class MissingArgumentMatcherPlaceholder : IArgumentMatcher {
        public string InvalidMatcherUsageMessage { get; }

        public MissingArgumentMatcherPlaceholder(ProxyMethodCall methodCall, int argumentPosition) {
            this.InvalidMatcherUsageMessage = CreateMessage(methodCall, argumentPosition);
        }

        public string Describe()
            => throw new NotImplementedException();

        private static string CreateMessage(ProxyMethodCall methodCall, int argumentPosition)
            =>  $"Invalid usage of argument matchers detected in call to " +
                $"{methodCall.Proxy.ProxiedType.Name}.{methodCall.CalledMethod.Name} method. " +
                $"Missing argument matcher for parameter on position {argumentPosition}. " +
                $"Please remember that all default values (null, 0, false, default) " +
                $"must be replaced by argument matchers in *setup* calls.";
    }
}