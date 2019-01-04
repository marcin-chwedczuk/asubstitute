namespace ASubstitute {
    public interface IArgumentMatcher<T> : IArgumentMatcher {
        bool Matches(T argumentValue);
    }
    public interface IArgumentMatcher { 
        
    }
}