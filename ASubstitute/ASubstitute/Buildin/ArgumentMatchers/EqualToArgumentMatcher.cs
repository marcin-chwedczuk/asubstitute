﻿using System.Collections.Generic;
using ASubstitute.Api;

namespace ASubstitute.Buildin.ArgumentMatchers {
    public class EqualToArgumentMatcher<T> : IArgumentMatcher<T> {
        private readonly T _value;

        public EqualToArgumentMatcher(T value) {
            _value = value;
        }

        public bool Matches(T argumentValue) {
            return AreEqual(_value, argumentValue);
        }
        
        public override bool Equals(object obj) {
            return 
                (obj is EqualToArgumentMatcher<T> other) && 
                AreEqual(this._value, other._value);
        }

        public override int GetHashCode()
            => _value?.GetHashCode() ?? 0;

        private static bool AreEqual(T a, T b)
            => EqualityComparer<T>.Default.Equals(a, b);
    }
}