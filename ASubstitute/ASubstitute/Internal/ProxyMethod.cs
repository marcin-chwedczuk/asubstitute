using System.Reflection;
using ASubstitute.Api.BuildingBlocks;

namespace ASubstitute.Internal {
    public class ProxyMethod : IMethod {
        private readonly MethodInfo _method;

        public string Name 
            => _method.Name;

        public ProxyMethod(MethodInfo method) {
            _method = method;
        }
    }
}