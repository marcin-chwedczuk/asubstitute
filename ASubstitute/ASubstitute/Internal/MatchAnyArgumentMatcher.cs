namespace ASubstitute.Internal {
    public class MatchAnyArgumentMatcher<T> : IArgumentMatcher<T> {
        public bool Matches(T argumentValue) {
            return true;
        }

        public override bool Equals(object obj) {
            return (obj is MatchAnyArgumentMatcher<T>);
        }

        public override int GetHashCode()
            // All instances of this class are equal to each
            // other, so they must share the same hashcode value.
            => 7717;
    }

}