namespace ASubstitute {

    // TODO: Move to API package
    public interface IArgumentMatcher<T> : IArgumentMatcher {
        bool Matches(T argumentValue);
    }
    public interface IArgumentMatcher { 
        
    }
}