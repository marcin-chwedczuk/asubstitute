using System.Reflection;
using System.Threading;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    static class ThreadLocalContext {
        private static ThreadLocal<Context> _context = 
            new ThreadLocal<Context>(() => new Context());

        public static void Clear() 
            => _context.Value.Clear();
        
        public static void SetCurrentMethodCall(ProxyMethodCall methodCall) 
            => _context.Value.SetCurrentMethodCall(methodCall);

        public static void AddArgumentMatcher(IArgumentMatcher matcher)
            => _context.Value.AddArgumentMatcher(matcher);

        internal static void RegisterBehaviour(IMethodBehaviour behaviour) 
            => _context.Value.RegisterBehaviour(behaviour);
    }
}