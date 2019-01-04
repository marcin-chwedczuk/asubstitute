using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class MatchEqualToArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly T _value;

        public MatchEqualToArgumentMatcher(T value) {
            _value = value;
        }

        public bool Matches(T argumentValue) {
            return AreEqual(_value, argumentValue);
        }
        
        public override bool Equals(object obj) {
            return 
                (obj is MatchEqualToArgumentMatcher<T> other) && 
                AreEqual(this._value, other._value);
        }

        public override int GetHashCode()
            => _value?.GetHashCode() ?? 0;

        private static bool AreEqual(T a, T b)
            => EqualityComparer<T>.Default.Equals(a, b);
    }
}