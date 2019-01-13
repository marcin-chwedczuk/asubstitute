using System;
using ASubstitute.Api;

namespace ASubstitute.Test.ExtensibilityExample.DoExtension {
    public class DoMethodBehaviour<T> : IMethodBehaviour {
        private readonly Func<object[], T> _callback;

        public DoMethodBehaviour(Func<object[], T> callback) {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public object Invoke(object[] methodArguments) {
            return _callback(methodArguments);
        }
    }
}