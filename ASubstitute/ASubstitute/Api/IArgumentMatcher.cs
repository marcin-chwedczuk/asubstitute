namespace ASubstitute.Api {
    public interface IArgumentMatcher { 
        string Describe();
    }

    public interface IArgumentMatcher<T> : IArgumentMatcher {
        bool Matches(T argumentValue);
    }
}