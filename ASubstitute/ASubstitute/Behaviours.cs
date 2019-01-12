﻿using System;
using ASubstitute.Api;
using ASubstitute.Buildin.Behaviours;

namespace ASubstitute {
    public static class Behaviours {
        public static T Returns<T>(this T obj, T valueToReturn) {
            SubstituteContext.RegisterBehaviour(
                new ReturnMethodBehaviour(valueToReturn));

            return default(T);
        }

        public static T Throws<T>(this T returnedValue, Exception exceptionToThrow) {
            SubstituteContext.RegisterBehaviour(
                new ThrowMethodBehaviour(exceptionToThrow));

            return default(T);
        }
    }
}