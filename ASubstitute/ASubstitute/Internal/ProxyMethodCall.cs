using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    class ProxyMethodCall : IMethodCall {
        public Proxy Proxy { get; }

        public ProxyMethod CalledMethod { get; }

        public IImmutableList<TypedArgument> PassedArguments { get; }

        IMethod IMethodCall.CalledMethod => CalledMethod;

        IImmutableList<ITypedArgument> IMethodCall.PassedArguments => PassedArguments
            .Cast<ITypedArgument>()
            .ToImmutableList();

        private ProxyMethodCall(
                Proxy proxy, 
                ProxyMethod calledMethod, 
                IImmutableList<TypedArgument> passedArguments)
        {
            Proxy = proxy;
            CalledMethod = calledMethod;
            PassedArguments = passedArguments;
        }

        public static ProxyMethodCall From(Proxy proxy, MethodInfo method, object[] arguments) {
            var parametersTypes = method
                .GetParameters()
                .Select(p => p.ParameterType);

            var typedArgs = parametersTypes
                .Zip(arguments, 
                     (type, value) => new TypedArgument(type, value))
                .ToImmutableList();

            return new ProxyMethodCall(proxy, new ProxyMethod(method), typedArgs);
        }
    }
}