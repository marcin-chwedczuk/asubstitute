using System.Reflection;
using ASubstitute.Internal;

namespace ASubstitute {
    public static class Substitute {
        public static T For<T>() {
            return DispatchProxy.Create<T, Proxy>();
        }
    }

}