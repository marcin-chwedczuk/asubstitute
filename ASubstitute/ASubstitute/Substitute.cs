using System.Reflection;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Substitute {
        public static T For<T>() {
            T proxy = DispatchProxy.Create<T, Proxy>();
            (proxy as Proxy).Init(typeof(T));
            return proxy;
        }
    }

}