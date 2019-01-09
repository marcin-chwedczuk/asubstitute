using System;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public static class QueueExtensionMethods {
        public static T DequeueOrElse<T>(this Queue<T> queue, Func<T> defaultValueFactory) {
            if (queue.Count > 0) {
                return queue.Dequeue();
            }
            else {
                return defaultValueFactory();
            }
        }
    }
}