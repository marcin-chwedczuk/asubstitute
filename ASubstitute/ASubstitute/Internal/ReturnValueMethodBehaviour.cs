namespace ASubstitute.Internal {
    public class ReturnValueMethodBehaviour : IMethodBehaviour {
        private readonly object _value;

        public ReturnValueMethodBehaviour(object value) {
            this._value = value;
        }

        public object Invoke(object[] args) {
            return _value;
        }
    }

}