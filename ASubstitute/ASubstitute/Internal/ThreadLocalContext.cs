using System.Reflection;
using System.Threading;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public static class ThreadLocalContext {
        private static ThreadLocal<Context> _context = 
            new ThreadLocal<Context>(() => new Context());
        
        public static void RecordInvocation(Proxy proxy, MethodInfo selectedMethod, IEnumerable<TypedArgument> args) {
            _context.Value.CurrentProxy = proxy;
            _context.Value.SelectedMethod = selectedMethod;

            // TODO: Error checking
            _context.Value.MethodArguments.Clear();
            _context.Value.MethodArguments.AddRange(args);
        }

        public static void AddArgumentMatcher(IArgumentMatcher matcher) {
            _context.Value.ArgumentMatchers.Add(matcher);
        }

        internal static void RegisterBehaviour(IRecordedBehaviour behaviour) {
            var context = _context.Value;
            var proxy = _context.Value.CurrentProxy;

            // TODO: Create full matcher set

            SpecificMethodCall methodCall = 
                proxy.FindCompatibleMethodCall(context.SelectedMethod, context.ArgumentMatchers);

            if (methodCall == null) {
                methodCall = new SpecificMethodCall(
                    new MethodCallMatcher(
                        context.SelectedMethod.Name, 
                        context.SelectedMethod.GetParameters().Length,
                        context.ArgumentMatchers));

                proxy.MethodCalls.Add(methodCall);
            }

            methodCall.AddBehaviour(behaviour);
        }
    }

}