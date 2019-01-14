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

        public IImmutableList<object> PassedArguments { get; }

        IMethod IMethodCall.CalledMethod => CalledMethod;

        private ProxyMethodCall(
                Proxy proxy, 
                ProxyMethod calledMethod, 
                IImmutableList<object> passedArguments)
        {
            Proxy = proxy;
            CalledMethod = calledMethod;
            PassedArguments = passedArguments;
        }

        public static ProxyMethodCall From(Proxy proxy, MethodInfo method, object[] arguments) {
            // TODO: ImmutableList -> ImmutableArray
            return new ProxyMethodCall(
                proxy, 
                ProxyMethod.From(method), 
                arguments.ToImmutableList());
        }
    }
}