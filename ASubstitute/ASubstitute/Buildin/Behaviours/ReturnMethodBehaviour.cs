using ASubstitute.Api;

namespace ASubstitute.Buildin.Behaviours {
    public class ReturnMethodBehaviour : IMethodBehaviour {
        private readonly object _value;

        public ReturnMethodBehaviour(object value) {
            this._value = value;
        }

        public object Invoke(object[] args) {
            return _value;
        }
    }

}