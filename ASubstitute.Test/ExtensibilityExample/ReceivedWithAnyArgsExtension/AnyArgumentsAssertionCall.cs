using System;
using System.Linq;
using System.Collections.Immutable;
using ASubstitute.Api;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    public class AnyArgumentsAssertionCall: IAssertionCall {
        public IMock Mock { get; }

        public IMethod Method { get; }

        public IImmutableList<IArgumentMatcher> ArgumentMatchers { get; }

        public AnyArgumentsAssertionCall(IAssertionCall callWithAnyArgs) {
            Mock = callWithAnyArgs.Mock;
            Method = callWithAnyArgs.Method;
            ArgumentMatchers = GenerateAnyMatchers(callWithAnyArgs.Method);
        }

        private static IImmutableList<IArgumentMatcher> GenerateAnyMatchers(IMethod method) {
            return method
                .ParameterTypes
                .Select(t => CreateAnyMatcher(t))
                .ToImmutableList();
        }

        private static IArgumentMatcher CreateAnyMatcher(Type type) {
            var matcher = typeof(SubstituteBuildin)
                .GetMethod(nameof(SubstituteBuildin.AnyArgumentMatcher))
                .MakeGenericMethod(type)
                .Invoke(null, Array.Empty<object>());

            return (IArgumentMatcher) matcher;
        }
    }
}