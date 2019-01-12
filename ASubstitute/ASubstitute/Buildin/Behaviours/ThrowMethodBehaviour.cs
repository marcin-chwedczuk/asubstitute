using System;
using ASubstitute.Api;

namespace ASubstitute.Buildin.Behaviours {
    public class ThrowMethodBehaviour : IMethodBehaviour {
        private readonly Exception _exception;

        public ThrowMethodBehaviour(Exception exception) {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            this._exception = exception;
        }

        public object Invoke(object[] args) {
            throw _exception;
        }
    }
}