﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static WinEventHook.NativeMethods;

namespace WinEventHook {
    /// <summary>
    /// Represents a hook that the system calls in response to events generated by an accessible object.
    /// A message loop is required for the hook to function propertly.
    /// </summary>
    public class WindowEventHook : IDisposable {
        private const uint AllThreads = 0;
        private const uint AllProcesses = 0;

        /// <summary>
        /// The lower bound of the observed event range.
        /// </summary>
        public WindowEvent EventMin { get; }
        /// <summary>
        /// The upper bound of the observed event range.
        /// </summary>
        public WindowEvent EventMax { get; }

        /// <summary>
        /// A value indicating whether the hook is currently attached.
        /// </summary>
        public bool Hooked { get; private set; }

        /// <summary>
        /// A value indicating whether the hook should observe events originating from the current thread.
        /// </summary>
        public bool SkipOwnThread {
            get => (_hookFlags & WinEventHookFlags.WINEVENT_SKIPOWNTHREAD) == WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;
            set {
                if (Hooked)
                    throw new InvalidOperationException("SkipOwnThread cannot be changed while hooked.");
                _hookFlags = value ? _hookFlags | WinEventHookFlags.WINEVENT_SKIPOWNTHREAD : _hookFlags & ~WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;
            }
        }
        /// <summary>
        /// A value indicating whether the hook should observe events originating from the current process.
        /// </summary>
        public bool SkipOwnProcess {
            get => (_hookFlags & WinEventHookFlags.WINEVENT_SKIPOWNPROCESS) == WinEventHookFlags.WINEVENT_SKIPOWNPROCESS;
            set {
                if (Hooked)
                    throw new InvalidOperationException("SkipOwnProcess cannot be changed while hooked.");
                _hookFlags = value ? _hookFlags | WinEventHookFlags.WINEVENT_SKIPOWNPROCESS : _hookFlags & ~WinEventHookFlags.WINEVENT_SKIPOWNPROCESS;
            }
        }
        private WinEventHookFlags _hookFlags = WinEventHookFlags.WINEVENT_OUTOFCONTEXT | WinEventHookFlags.WINEVENT_SKIPOWNPROCESS | WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;

        /// <summary>
        /// Occurs whenever an window event is raised in an oberserved process or thread.
        /// </summary>
        public event EventHandler<WinEventHookEventArgs>? EventReceived;

        /// <summary>
        /// The handle representing this hook.
        /// </summary>
        public IntPtr RawHookHandle { get; private set; }

        private WinEventProc? eventHandler;

        /// <summary>
        /// Creates a new hook that listens to all events.
        /// </summary>
        public WindowEventHook() : this(WindowEvent.EVENT_MIN, WindowEvent.EVENT_MAX) { }
        /// <summary>
        /// Creates a new hook that listens to the specified event.
        /// </summary>
        public WindowEventHook(WindowEvent @event) : this(@event, @event) { }
        /// <summary>
        /// Creates a new hook that listens to the specified event range.
        /// </summary>
        public WindowEventHook(WindowEvent eventMin, WindowEvent eventMax) {
            EventMin = eventMin;
            EventMax = eventMax;
        }

        /// <summary>
        /// Attaches the hook to all running processes and threads.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this hook is already hooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public void HookGlobal() =>
            HookInternal(processId: AllProcesses, threadId: AllThreads, throwIfAlreadyHooked: true, throwOnFailure: true);

        /// <summary>
        /// Trys to attach the hook to all running processes and threads.
        /// </summary>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        public bool TryHookGlobal() =>
            HookInternal(processId: AllProcesses, threadId: AllThreads, throwIfAlreadyHooked: false, throwOnFailure: false);

        /// <summary>
        /// Attaches the hook to the specfied process.
        /// </summary>
        /// <param name="process">The process that should be observed.</param>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        /// <exception cref="InvalidOperationException">If this hook is already hooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public void HookToProcess(Process process) =>
            HookToProcess((uint)process.Id);

        /// <summary>
        /// Trys to attach the hook to the specfied process.
        /// </summary>
        /// <param name="process">The process that should be observed.</param>
        public bool TryHookToProcess(Process process) =>
            TryHookToProcess((uint)process.Id);

        /// <summary>
        /// Attaches the hook to the process specfied by the given id.
        /// </summary>
        /// <param name="processId">The id of the process that should be observed.</param>
        /// <exception cref="InvalidOperationException">If this hook is already hooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public void HookToProcess(uint processId) =>
            HookInternal(processId, threadId: AllThreads, throwIfAlreadyHooked: true, throwOnFailure: true);

        /// <summary>
        /// Trys to attach the hook to the process specfied by the given id.
        /// </summary>
        /// <param name="processId">The id of the process that should be observed.</param>
        public bool TryHookToProcess(uint processId) =>
            HookInternal(processId, threadId: AllThreads, throwIfAlreadyHooked: false, throwOnFailure: false);

        /// <summary>
        /// Attaches the hook to the specfied thread.
        /// </summary>
        /// <param name="thread">The thread that should be observed.</param>
        /// <exception cref="InvalidOperationException">If this hook is already hooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public void HookToThread(ProcessThread thread) =>
            HookToThread((uint)thread.Id);

        /// <summary>
        /// Attaches the hook to the specfied thread.
        /// </summary>
        /// <param name="thread">The thread that should be observed.</param>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        public bool TryHookToThread(ProcessThread thread) =>
            TryHookToThread((uint)thread.Id);

        /// <summary>
        /// Attaches the hook to the thread specfied by the given id.
        /// </summary>
        /// <param name="threadId">The id of the thread that should be observed.</param>
        /// <exception cref="InvalidOperationException">If this hook is already hooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public void HookToThread(uint threadId) =>
            HookInternal(processId: AllProcesses, threadId, throwIfAlreadyHooked: true, throwOnFailure: true);

        /// <summary>
        /// Attaches the hook to the thread specfied by the given id.
        /// </summary>
        /// <param name="threadId">The id of the thread that should be observed.</param>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        public bool TryHookToThread(uint threadId) =>
            HookInternal(processId: AllProcesses, threadId, throwIfAlreadyHooked: false, throwOnFailure: false);

        /// <summary>
        /// Attaches the hook globally or to the specified thread or process.
        /// </summary>
        /// <param name="processId">The id of the process to attach to.</param>
        /// <param name="threadId">The id of the thread to attach to.</param>
        /// <param name="throwIfAlreadyHooked">Should an exception be thrown if the hook is already in the hooked state.</param>
        /// <param name="throwOnFailure">Should an exception be thrown if the operation fails.</param>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        /// <exception cref="InvalidOperationException">If this hook is already hooked and <paramref name="throwIfAlreadyHooked"/> is true.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation and <paramref name="throwOnFailure"/> is true.</exception>
        internal bool HookInternal(uint processId = AllProcesses, uint threadId = AllThreads, bool throwIfAlreadyHooked = true, bool throwOnFailure = true) {
            if (Hooked) {
                if (throwIfAlreadyHooked)
                    throw new InvalidOperationException("Hook is already hooked.");
                return true;
            }

            eventHandler = new WinEventProc(OnWinEventProc);
            RawHookHandle = SetWinEventHook(
                eventMin: EventMin,
                eventMax: EventMax,
                hmodWinEventProc: IntPtr.Zero,
                lpfnWinEventProc: eventHandler,
                idProcess: processId,
                idThread: threadId,
                dwFlags: _hookFlags
            );

            if (RawHookHandle != IntPtr.Zero) {
                Hooked = true;
                return true;
            } else {
                eventHandler = null;
                if (throwOnFailure)
                    throw new Win32Exception();
                return false;
            }
        }

        /// <summary>
        /// Detaches the hook from wherever it is attached to.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this hook is already unhooked.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation.</exception>
        public bool Unhook() =>
            UnhookInternal(throwIfNotHooked: true, throwOnFailure: true);

        /// <summary>
        /// Detaches the hook from wherever it is attached to.
        /// </summary>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        public bool TryUnhook() =>
            UnhookInternal(throwIfNotHooked: false, throwOnFailure: false);

        /// <summary>
        /// Detaches the hook from wherever it is attached to.
        /// </summary>
        /// <param name="throwIfNotHooked">Should an exception be thrown if the hook is not in the hooked state.</param>
        /// <param name="throwOnFailure">Should an exception be thrown if the operation fails.</param>
        /// <returns>A value indication whether the operation completed successfully.</returns>
        /// <exception cref="InvalidOperationException">If this hook is already unhooked and <paramref name="throwIfNotHooked"/> is true.</exception>
        /// <exception cref="Win32Exception">If an error occured during the operation and <paramref name="throwOnFailure"/> is true.</exception>
        internal bool UnhookInternal(bool throwIfNotHooked = true, bool throwOnFailure = true) {
            if (!Hooked) {
                if (throwIfNotHooked)
                    throw new InvalidOperationException("Hook is not hooked.");
                return true;
            }

            // we need to unhook before freeing our callback in case an event sneaks in at the right time.
            var result = UnhookWinEvent(RawHookHandle);


            // we assume that we are no longer hooked even after unhooking failed.
            Hooked = false;
            eventHandler = null;
            RawHookHandle = IntPtr.Zero;

            if (!result && throwOnFailure)
                throw new Win32Exception();

            return result;
        }

        protected virtual void OnWinEventProc(IntPtr hWinEventHook, WindowEvent eventType, IntPtr hwnd, AccessibleObjectID idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
            if (hWinEventHook != RawHookHandle)
               return;

            EventReceived?.Invoke(this, new WinEventHookEventArgs(hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime));
        }

        #region IDisposable
        private bool _disposed;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                UnhookInternal(throwIfNotHooked: false, throwOnFailure: false);

                _disposed = true;
            }
        }

        ~WindowEventHook() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// Represents an event generated by an accessible object.
    /// </summary>
    public sealed class WinEventHookEventArgs : EventArgs {
        private const int CHILDID_SELF = 0;

        /// <summary>
        /// Handle to an event hook callback.
        /// </summary>
        public IntPtr HookHandle { get; }

        /// <summary>
        /// Specifies the event that occurred.
        /// </summary>
        public WindowEvent EventType { get; }

        /// <summary>
        /// Handle to the window that generates the event, or null if no window is associated with the event. For example, the mouse pointer is not associated with a window.<
        /// </summary>
        public IntPtr WindowHandle { get; }

        /// <summary>
        /// Identifies the object associated with the event. This is one of the object identifiers or a custom object ID.
        /// </summary>
        public AccessibleObjectID ObjectId { get; }

        /// <summary>
        /// Identifies the id of the child that triggered the event if it was not generated by the object itself.
        /// You can determine if the event originated from a child object with <see cref="IsChildEvent"/>.
        /// </summary>
        public int ChildId { get; }

        /// <summary>
        /// The id of the thread that generated this event.
        /// </summary>
        /// <remarks>
        /// This does not refer to managed thread ids.
        /// </remarks>
        public uint EventThreadId { get; }

        /// <summary>
        /// Time in milliseconds when the event was generated since the system started.
        /// </summary>
        public uint EventTime { get; }

        /// <summary>
        /// The date and time the event was generated.
        /// </summary>
        public DateTime EventDate => DateTime.Now.AddMilliseconds(EventTime - Environment.TickCount);

        /// <summary>
        /// A value indicating whether the event was triggered by a child of the given object.
        /// The child can be identified via <see cref="ChildId"/>.
        /// </summary>
        public bool IsChildEvent => !IsOwnEvent;

        /// <summary>
        /// A value indicating whether the event was triggered by given object (not a child).
        /// </summary>
        public bool IsOwnEvent => ChildId == CHILDID_SELF;

        /// <summary>
        /// Constructs a new <see cref="WinEventHookEventArgs"/> instance witht the given data.
        /// </summary>
        /// <param name="hookHandle">The handle to the event hook callback.</param>
        /// <param name="eventType">The type of the event that occured.</param>
        /// <param name="windowHandle">The handle of the window that generated the event.</param>
        /// <param name="objectId">Identifies the object associated with the event</param>
        /// <param name="childId">Identifies the id of the child that triggered the event if it was not generated by the object itself.</param>
        /// <param name="eventThreadId">The id of the thread that generated this event.</param>
        /// <param name="eventTime">Time in milliseconds when the event was generated since the system started.</param>
        public WinEventHookEventArgs(IntPtr hookHandle, WindowEvent eventType, IntPtr windowHandle, AccessibleObjectID objectId, int childId, uint eventThreadId, uint eventTime) {
            HookHandle = hookHandle;
            EventType = eventType;
            WindowHandle = windowHandle;
            ObjectId = objectId;
            ChildId = childId;
            EventThreadId = eventThreadId;
            EventTime = eventTime;
        }
    }
}
