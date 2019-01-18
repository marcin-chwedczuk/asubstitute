using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Api;

namespace ASubstitute.Internal {
    class ProxyMethodCall : IMethodCall {
        public IMock Mock { get; }

        public IMethod CalledMethod { get; }

        public IImmutableList<object> PassedArguments { get; }

        private ProxyMethodCall(
                IMock proxy, 
                IMethod calledMethod, 
                IImmutableList<object> passedArguments)
        {
            Mock = proxy;
            CalledMethod = calledMethod;
            PassedArguments = passedArguments;
        }

        public static ProxyMethodCall From(IMock mock, MethodInfo method, object[] arguments) {
            // TODO: ImmutableList -> ImmutableArray
            return new ProxyMethodCall(
                mock, 
                ProxyMethod.From(mock, method), 
                arguments.ToImmutableList());
        }
    }
}