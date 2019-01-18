using System;
using System.Linq;
using System.Collections.Immutable;
using System.Reflection;
using ASubstitute.Api.BuildingBlocks;
using ASubstitute.Api;

namespace ASubstitute.Internal {
    public class ProxyMethod : IMethod {
        public string ProxyName { get; }

        public string Name { get; }

        public IImmutableList<Type> ParameterTypes { get; }

        private ProxyMethod(string proxyName, string name, IImmutableList<Type> parameterTypes) {
            ProxyName = proxyName;
            Name = name;
            ParameterTypes = parameterTypes;
        }

        public static ProxyMethod From(IMock mock, MethodInfo method) {
            var parameterTypes = method
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToImmutableList();
 
            return new ProxyMethod(mock.Name, method.Name, parameterTypes);
        }
    }
}