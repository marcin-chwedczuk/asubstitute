using System;
using System.Linq;
using Xunit;
using System.Reflection;
using Xunit.Abstractions;
using System.Threading;
using System.Collections.Generic;

namespace ASubstitute.Test
{
    public class ASubstituteTest
    {
        [Fact]
        public void Test1()
        {
            var foo = DispatchProxy.Create<IFoo, AProxy>();

            foo
                .Foo(Arg.Any<int>(), Arg.Is("foo"), Arg.Any<object>())
                .Returns(13)
                .Returns(17)
                .Returns(18);

            int r = foo.Foo(3, "foo", null);
            Assert.Equal(13, r);

            int b1 = foo.Foo(3, "bar", null);
            Assert.Equal(default(int), b1);

            int r2 = foo.Foo(3, "foo", null);
            Assert.Equal(17, r2);

            int r3 = foo.Foo(3, "foo", null);
            Assert.Equal(18, r3);

            int r4 = foo.Foo(3, "foo", null);
            Assert.Equal(18, r3);

            int b2 = foo.Foo(3, "bar", null);
            Assert.Equal(default(int), b2);
        }
    }

    public class AContext {
        public AProxy CurrentProxy { get; set; }
        public MethodInfo SelectedMethod { get; set; }

        public List<IArgumentMatcher> ArgumentMatchers { get; } 
            = new List<IArgumentMatcher>();

        public List<TypedArgument> MethodArguments { get; }
            = new List<TypedArgument>();
    }

    public class TypedArgument {
        public Type Type { get; }
        public object Value { get; }

        public TypedArgument(Type type, object value) {
            Type = type;
            Value = value;
        }
    }

    public static class AThreadLocalContext {
        private static ThreadLocal<AContext> _context = 
            new ThreadLocal<AContext>(() => new AContext());
        
        public static void RecordInvocation(AProxy proxy, MethodInfo selectedMethod, IEnumerable<TypedArgument> args) {
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

    public class AProxy : DispatchProxy {
        public IList<SpecificMethodCall> MethodCalls { get; } = new List<SpecificMethodCall>();

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            var typedArgs = targetMethod.GetParameters()
                .Select((param, index) => new TypedArgument(param.ParameterType, args[index]))
                .ToArray();
                
            AThreadLocalContext.RecordInvocation(this, targetMethod, typedArgs);

            var methodCall = FindFirstMatching(targetMethod, typedArgs);
            if (methodCall != null) {
                var result = methodCall.InvokeBehaviour(typedArgs);

                if (!object.ReferenceEquals(result, SpecificMethodCall.NO_RESULT)) {
                    return result;
                }
            }

            // return default(T) 
            return targetMethod.ReturnType.IsValueType && targetMethod.ReturnType != typeof(void)
                ? Activator.CreateInstance(targetMethod.ReturnType)
                : null;
        }

        public SpecificMethodCall FindFirstMatching(MethodInfo method, IList<TypedArgument> typedArgs) {
            return MethodCalls
                .FirstOrDefault(x => x.MatchesCall(method, typedArgs));
        }

        public SpecificMethodCall FindCompatibleMethodCall(MethodInfo method, IList<IArgumentMatcher> matchers) {
            return MethodCalls
                .FirstOrDefault(x => x.IsCompatible(method, matchers));
        }
    }

    public static class Arg {
        public static T Any<T>() {
            AThreadLocalContext.AddArgumentMatcher(
                new MatchAnyArgumentMatcher<T>());

            return default(T);
        }

        public static T Is<T>(T value) {
            AThreadLocalContext.AddArgumentMatcher(
                new IsEqualToArgumentMatcher<T>(value));

            return default(T);
        }
    }

    public interface IArgumentMatcher { 
        
    }

    public interface IArgumentMatcher<T> : IArgumentMatcher {
        bool Matches(T argumentValue);
    }

    public class MatchAnyArgumentMatcher<T> : IArgumentMatcher<T> {
        public bool Matches(T argumentValue) {
            return true;
        }

        public override bool Equals(object obj) {
            return (obj is MatchAnyArgumentMatcher<T>);
        }
    }

    public class IsEqualToArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly T _value;

        public IsEqualToArgumentMatcher(T value) {
            _value = value;
        }

        public bool Matches(T argumentValue) {
            return EqualityComparer<T>.Default.Equals(_value, argumentValue);
        }

        
        public override bool Equals(object obj) {
            return (obj is IsEqualToArgumentMatcher<T> other) && 
                EqualityComparer<T>.Default.Equals(this._value, other._value);
        }
    }

    public static class Actions {
        public static T Returns<T>(this T obj, T valueToReturn) {
            AThreadLocalContext.RegisterBehaviour(
                new ReturnValueRecordedBehaviour(valueToReturn));

            return default(T);
        }
    }

    public interface IRecordedBehaviour {
        object Invoke(object[] args);
    }

    public class ReturnValueRecordedBehaviour : IRecordedBehaviour {
        private readonly object _value;

        public ReturnValueRecordedBehaviour(object value) {
            this._value = value;
        }

        public object Invoke(object[] args) {
            return _value;
        }
    }

    public class MethodCallMatcher {
        private readonly string _methodName;
        private readonly int _parametersCount;

        private readonly IList<IArgumentMatcher> _argumentMatchers;

        public MethodCallMatcher(
            string methodName, 
            int parametersCount, 
            IList<IArgumentMatcher> argumentMatchers)
        {
            _methodName = methodName;
            _parametersCount = parametersCount;
            _argumentMatchers = argumentMatchers;   

            if (_parametersCount != _argumentMatchers.Count)
                throw new ArgumentException(
                    $"Expecting to get {_parametersCount} argument matchers, instead got {_argumentMatchers.Count}.");
        }

        public bool MatchesCall(MethodInfo method, IList<TypedArgument> arguments) {
            if (string.CompareOrdinal(method.Name, _methodName) != 0) 
                return false;

            if (arguments.Count != _parametersCount)
                return false;

            for (int i = 0; i < arguments.Count; i++) {
                TypedArgument arg = arguments[i];
                IArgumentMatcher matcher = _argumentMatchers[i];

                // Call Matches<arg.Type>(arg.Value, matcher)
                var matchesMethod = typeof(MethodCallMatcher).GetMethod(nameof(Matches));
                MethodInfo generic = matchesMethod.MakeGenericMethod(arg.Type);
                var result = (bool) generic.Invoke(this, new object[] { arg.Value, matcher });

                if (!result) return false;
            }

            return true;
        }

        public static bool Matches<T>(T argumentValue, IArgumentMatcher matcher) {
            if (matcher is IArgumentMatcher<T> typedMatcher) {
                return typedMatcher.Matches(argumentValue);
            }

            return false;
        }

        
        public bool IsCompatible(MethodInfo method, IList<IArgumentMatcher> matchers) {
            if (string.CompareOrdinal(method.Name, _methodName) != 0) 
                return false;

            if (matchers.Count != _parametersCount)
                return false;

            for (int i = 0; i < matchers.Count; i++) {
                if (!matchers[i].Equals(_argumentMatchers[i]))
                    return false;
            }

            return true;
        }
    }

    public class SpecificMethodCall {
        public static readonly object NO_RESULT = new object();

        public readonly MethodCallMatcher _methodCallMatcher;
        public readonly Queue<IRecordedBehaviour> _recordedBehaviours
            = new Queue<IRecordedBehaviour>();

        public SpecificMethodCall(MethodCallMatcher methodCallMatcher)
        {
            _methodCallMatcher = methodCallMatcher;
        }

        public void AddBehaviour(IRecordedBehaviour behaviour) {
            _recordedBehaviours.Enqueue(behaviour);
        }

        public bool MatchesCall(MethodInfo method, IList<TypedArgument> arguments) {
            return _methodCallMatcher.MatchesCall(method, arguments);
        }

        public object InvokeBehaviour(IList<TypedArgument> arguments) {
            IRecordedBehaviour behaviour = _recordedBehaviours.Count > 1
                ? _recordedBehaviours.Dequeue()
                : _recordedBehaviours.Peek();

            return behaviour.Invoke(
                arguments.Select(x => x.Value).ToArray());
        }

        internal bool IsCompatible(MethodInfo method, IList<IArgumentMatcher> matchers) {
            return _methodCallMatcher.IsCompatible(method, matchers);
        }
    }

    public interface IFoo {
        int Foo(int a, string b, object c);
    }
}
