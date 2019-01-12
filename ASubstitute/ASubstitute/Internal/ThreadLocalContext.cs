using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using ASubstitute.Api;

namespace ASubstitute.Internal {
    static class ThreadLocalContext {
        private static ThreadLocal<Context> _context = 
            new ThreadLocal<Context>(() => new Context());

        public static void Clear() 
            => _context.Value.Clear();
        
        public static MethodCallMatcher SetCurrentMethodCall(ProxyMethodCall methodCall) 
            => _context.Value.SetCurrentMethodCall(methodCall);

        public static void AddArgumentMatcher(IArgumentMatcher matcher)
            => _context.Value.AddArgumentMatcher(matcher);

        public static void RegisterBehaviour(IMethodBehaviour behaviour) 
            => _context.Value.RegisterBehaviour(behaviour);

        internal static void RegisterAssertion(IMethodCallHistoryAssertion assertion)
            => _context.Value.RegisterAssertion(assertion);

        internal static IMethodCallHistoryAssertion ConsumeAssertion()
            => _context.Value.ConsumeAssertion();
    }
}