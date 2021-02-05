using System;
using System.Collections.Generic;
using System.Threading;
using WinEventHook.Extensions;

namespace WinEventHook {
    /// <summary>
    /// An event processor that queues incoming events and processes them sequentially avoiding reentrancy in hook functions.
    /// </summary>
    /// <typeparam name="T">The type of the event data that represents a queued event</typeparam>
    public class ReentrancySafeEventProcessor<T> {
        private const int Processing = 1;
        private const int Idle = 0;

        private int _processing = Idle;
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
            // add event data to queue.
            _eventQueue.Enqueue(eventData);

            // check if already executing and start processing if this is the first call.
            if (Interlocked.Exchange(ref _processing, Processing) == Processing) {
                // some call already reached processing and will handle our event.
                return;
            }

            // process queued events
            ProcessQueue();
        }

        private void ProcessQueue() {
            try {
                // process queued events
                while (_eventQueue.TryDequeue(out var data)) {
                    _eventProcessor(data);
                }
            } finally {
                // stop processing
                _processing = Idle;

                // if someone added an event after we finished and nobody has started processing yet we start again.
                if (!_eventQueue.IsEmpty() && Interlocked.Exchange(ref _processing, Processing) == Processing) {
                    ProcessQueue();
                }
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
