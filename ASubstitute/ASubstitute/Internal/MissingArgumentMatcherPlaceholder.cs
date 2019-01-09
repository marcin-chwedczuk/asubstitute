namespace ASubstitute.Internal {
    public class MissingArgumentMatcherPlaceholder : IArgumentMatcher {
        public string Message { get; }

        public MissingArgumentMatcherPlaceholder(string message) {
            this.Message = message;
        }
    }
}