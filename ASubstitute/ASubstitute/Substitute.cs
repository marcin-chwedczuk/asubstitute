using System.Reflection;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Substitute {
        public static T For<T>() {
            return For<T>(substituteName: typeof(T).Name);
        }

        public static T For<T>(string substituteName) {
            T proxy = DispatchProxy.Create<T, Proxy>();
            (proxy as Proxy).Init(typeof(T), substituteName);
            return proxy;
        }
    }
}