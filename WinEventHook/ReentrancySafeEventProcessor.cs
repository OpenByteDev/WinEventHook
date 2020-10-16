using System;
using System.Collections.Generic;
using WinEventHook.Extensions;

namespace WinEventHook {
    /// <summary>
    /// An event processor that queues incoming events and processes them sequentially avoiding reentrancy in hook functions.
    /// </summary>
    /// <typeparam name="T">The type of the event data that represents a queued event</typeparam>
    public class ReentrancySafeEventProcessor<T> {

        private bool _processing;
        private readonly Queue<T> _eventQueue = new Queue<T>();
        private readonly Action<T> _eventProcessor;

        /// <summary>
        /// Creates a new event processor with the specified event handler.
        /// </summary>
        /// <param name="eventProcessor"></param>
        public ReentrancySafeEventProcessor(Action<T> eventProcessor) {
            _eventProcessor = eventProcessor;
        }

        /// <summary>
        /// Enqueues the given event and starts processing of queued events if necessary.
        /// </summary>
        /// <param name="eventData">An object representing the event</param>
        public void EnqueueAndProcess(T eventData) {
            // check if already executing
            if (_processing) {
                // some call already reached processing.
                _eventQueue.Enqueue(eventData);
                return;
            }

            // first call, so we start executing
            _processing = true;

            try {
                // process own event data
                _eventProcessor(eventData);

                // process queued events
                while (_eventQueue.TryDequeue(out var data)) {
                    _eventProcessor(data);
                }
            } finally {
                // stop executing
                _processing = false;
            }
        }

        /// <summary>
        /// Flush the current event queue.
        /// </summary>
        public void FlushQueue() {
            _eventQueue.Clear();
        }
    }
}
