using System;

namespace ASubstitute.Internal {
    public class ThrowExceptionMethodBehaviour : IMethodBehaviour {
        private readonly Exception exception;

        public ThrowExceptionMethodBehaviour(Exception exception) {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            this.exception = exception;
        }

        public object Invoke(object[] args) {
            throw exception;
        }
    }
}