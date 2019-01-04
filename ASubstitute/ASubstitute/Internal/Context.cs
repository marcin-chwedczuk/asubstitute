using System.Reflection;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public class Context {
        public Proxy CurrentProxy { get; set; }
        public MethodInfo SelectedMethod { get; set; }

        public List<IArgumentMatcher> ArgumentMatchers { get; } 
            = new List<IArgumentMatcher>();

        public List<TypedArgument> MethodArguments { get; }
            = new List<TypedArgument>();
    }

}