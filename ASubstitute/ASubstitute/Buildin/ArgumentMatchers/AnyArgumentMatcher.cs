using ASubstitute.Api;
using ASubstitute.Internal;

namespace ASubstitute.Buildin.ArgumentMatchers {
    public class AnyArgumentMatcher<T> : IArgumentMatcher<T> {
        public bool Matches(T argumentValue)
            => true;    
        public string Describe()
            => $"any<{ReflectionUtils.GetCSharpNameOf(typeof(T))}>";

        public override bool Equals(object obj) {
            return (obj is AnyArgumentMatcher<T>);
        }

        public override int GetHashCode()
            // All instances of this class are equal to each
            // other, so they must share the same hashcode value.
            => 7717;
    }
}