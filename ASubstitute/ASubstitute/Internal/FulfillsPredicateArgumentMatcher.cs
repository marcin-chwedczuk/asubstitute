using System;

namespace ASubstitute.Internal {
    public class FulfillsPredicateArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly Predicate<T> predicate;

        public FulfillsPredicateArgumentMatcher(Predicate<T> predicate) {
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool Matches(T argumentValue) {
            return predicate(argumentValue);
        }
        
        public override bool Equals(object obj) {
            return 
                (obj is FulfillsPredicateArgumentMatcher<T> other) && 
                object.Equals(this.predicate, other.predicate);
        }

        public override int GetHashCode()
            => predicate.GetHashCode();
    }
}