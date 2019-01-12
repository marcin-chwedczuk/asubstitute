namespace ASubstitute.Api {
    public interface IArgumentMatcher { }

    public interface IArgumentMatcher<T> : IArgumentMatcher {
        bool Matches(T argumentValue);
    }
}