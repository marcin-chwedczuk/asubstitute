using System;
using System.Linq;
using System.Collections.Immutable;
using System.Reflection;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    public class ProxyMethod : IMethod {
        public string Name { get; }

        public IImmutableList<Type> ParameterTypes { get; }

        private ProxyMethod(string name, IImmutableList<Type> parameterTypes) {
            Name = name;
            ParameterTypes = parameterTypes;
        }

        public static ProxyMethod From(MethodInfo method) {
            var parameterTypes = method
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToImmutableList();
 
            return new ProxyMethod(method.Name, parameterTypes);
        }
    }
}