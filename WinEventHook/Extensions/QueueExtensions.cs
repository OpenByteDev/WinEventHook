using System.Collections.Generic;

namespace WinEventHook.Extensions {
    internal static class QueueExtensions {
#if NET45 || NETSTANDARD2_0
        public static bool TryDequeue<T>(this Queue<T> queue, out T result) {
            if (queue.Count == 0) {
                result = default!;
                return false;
            } else {
                result = queue.Dequeue();
                return true;
            }
        }
#endif

        public static bool IsEmpty<T>(this Queue<T> queue) => queue.Count == 0;
    }
}
