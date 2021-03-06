﻿using System.Collections.Generic;
using ASubstitute.Api;

namespace ASubstitute.Buildin.ArgumentMatchers {
    public class IsArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly T _value;

        public IsArgumentMatcher(T value) {
            _value = value;
        }

        public bool Matches(T argumentValue)
            => AreEqual(_value, argumentValue);

        public string Describe()
            => _value?.ToString() ?? "null";

        public override bool Equals(object obj)
            => (obj is IsArgumentMatcher<T> other) && 
                AreEqual(this._value, other._value);

        public override int GetHashCode()
            => _value?.GetHashCode() ?? 0;

        private static bool AreEqual(T a, T b)
            => EqualityComparer<T>.Default.Equals(a, b);
    }
}