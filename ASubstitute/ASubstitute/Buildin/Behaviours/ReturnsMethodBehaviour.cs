using ASubstitute.Api;

namespace ASubstitute.Buildin.Behaviours {
    public class ReturnsMethodBehaviour : IMethodBehaviour {
        private readonly object _value;

        public ReturnsMethodBehaviour(object value) {
            this._value = value;
        }

        public object Invoke(object[] args) {
            return _value;
        }
    }

}