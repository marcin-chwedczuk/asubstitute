namespace ASubstitute.Internal {
    public class ReturnValueRecordedBehaviour : IRecordedBehaviour {
        private readonly object _value;

        public ReturnValueRecordedBehaviour(object value) {
            this._value = value;
        }

        public object Invoke(object[] args) {
            return _value;
        }
    }

}