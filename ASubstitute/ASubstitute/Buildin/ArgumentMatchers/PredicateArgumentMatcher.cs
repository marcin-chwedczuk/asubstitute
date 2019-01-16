using System;
using ASubstitute.Api;

namespace ASubstitute.Buildin.ArgumentMatchers {
    public class PredicateArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly Predicate<T> predicate;

        public PredicateArgumentMatcher(Predicate<T> predicate) {
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool Matches(T argumentValue)
            => predicate(argumentValue);

        public string Describe()
            => predicate.ToString();
        
        public override bool Equals(object obj) {
            return 
                (obj is PredicateArgumentMatcher<T> other) && 
                object.Equals(this.predicate, other.predicate);
        }

        public override int GetHashCode()
            => predicate.GetHashCode();
    }
}