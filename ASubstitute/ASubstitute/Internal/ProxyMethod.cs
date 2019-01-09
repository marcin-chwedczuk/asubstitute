using System.Reflection;

namespace ASubstitute.Internal {
    public class ProxyMethod {
        private readonly MethodInfo _method;

        public string Name 
            => _method.Name;

        public ProxyMethod(MethodInfo method) {
            _method = method;
        }

        public int GetParametersCount()
            => _method.GetParameters().Length;
    }
}