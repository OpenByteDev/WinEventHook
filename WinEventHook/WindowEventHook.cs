using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static WinEventHook.NativeMethods;

namespace WinEventHook {
    public class WindowEventHook : IDisposable {
        private const uint AllThreads = 0;
        private const uint AllProcesses = 0;

        public const int CHILDID_SELF = 0;

        public WindowEvent EventMin { get; }
        public WindowEvent EventMax { get; }
        public bool Hooked { get; private set; }

        public bool SkipOwnThread {
            get => (_hookFlags & WinEventHookFlags.WINEVENT_SKIPOWNTHREAD) == WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;
            set => _hookFlags |= WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;
        }
        public bool SkipOwnProcess {
            get => (_hookFlags & WinEventHookFlags.WINEVENT_SKIPOWNPROCESS) == WinEventHookFlags.WINEVENT_SKIPOWNPROCESS;
            set => _hookFlags |= WinEventHookFlags.WINEVENT_SKIPOWNPROCESS;
        }
        private WinEventHookFlags _hookFlags = WinEventHookFlags.WINEVENT_OUTOFCONTEXT | WinEventHookFlags.WINEVENT_SKIPOWNPROCESS | WinEventHookFlags.WINEVENT_SKIPOWNTHREAD;

        public event WinEventProc? WinEventProc;

        public IntPtr RawHookHandle { get; private set; }
        private GCHandle _eventProcHandle;

        public WindowEventHook() : this(WindowEvent.EVENT_MIN, WindowEvent.EVENT_MAX) { }
        public WindowEventHook(WindowEvent @event) : this(@event, @event) { }
        public WindowEventHook(WindowEvent eventMin, WindowEvent eventMax) {
            EventMin = eventMin;
            EventMax = eventMax;
        }

        public bool HookGlobal(bool throwIfAlreadyHooked = true, bool throwOnFailure = true) =>
            HookInternal(processId: AllProcesses, threadId: AllThreads, throwIfAlreadyHooked, throwOnFailure);
        public bool HookToProcess(Process process, bool throwIfAlreadyHooked = true, bool throwOnFailure = true) =>
            HookToProcess((uint) process.Id, throwIfAlreadyHooked, throwOnFailure);
        public bool HookToProcess(uint processId, bool throwIfAlreadyHooked = true, bool throwOnFailure = true) =>
            HookInternal(processId, threadId: AllThreads, throwIfAlreadyHooked, throwOnFailure);
        public bool HookToThread(Thread thread, bool throwIfAlreadyHooked = true, bool throwOnFailure = true) =>
            HookToThread((uint) thread.ManagedThreadId, throwIfAlreadyHooked, throwOnFailure);
        public bool HookToThread(uint threadId, bool throwIfAlreadyHooked = true, bool throwOnFailure = true) =>
            HookInternal(processId: AllProcesses, threadId, throwIfAlreadyHooked, throwOnFailure);
        protected bool HookInternal(uint processId, uint threadId, bool throwIfAlreadyHooked, bool throwOnFailure) {
            if (Hooked) {
                if (throwIfAlreadyHooked)
                    throw new InvalidOperationException("Hook is already hooked.");
                return true;
            }

            var eventProc = new WinEventProc(OnWinEventProc);
            _eventProcHandle = GCHandle.Alloc(eventProc);
            RawHookHandle = SetWinEventHook(
                eventMin: EventMin,
                eventMax: EventMax,
                hmodWinEventProc: IntPtr.Zero,
                lpfnWinEventProc: eventProc,
                idProcess: processId,
                idThread: threadId,
                dwFlags: _hookFlags
            );

            if (RawHookHandle != IntPtr.Zero) {
                Hooked = true;
                return true;
            } else {
                _eventProcHandle.Free();
                if (throwOnFailure)
                    throw new Win32Exception();
                return false;
            }
        }

        public bool Unhook(bool throwIfNotHooked = true, bool throwOnFailure = true) {
            if (!Hooked) {
                if (throwIfNotHooked)
                    throw new InvalidOperationException("Hook is not hooked.");
                return true;
            }

            if (_eventProcHandle.IsAllocated)
                _eventProcHandle.Free();

            if (UnhookWinEvent(RawHookHandle)) {
                Hooked = false;
                return true;
            } else {
                if (throwOnFailure)
                    throw new Win32Exception();
                return false;
            }
        }

        protected virtual void OnWinEventProc(IntPtr hWinEventHook, WindowEvent eventType, IntPtr hwnd, AccessibleObjectID idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
            WinEventProc?.Invoke(hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwEventThread);
        }

        #region IDisposable
        private bool _disposed;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    // dispose managed state
                }

                // free unmanaged resources
                Unhook(throwIfNotHooked: false, throwOnFailure: false);

                _disposed = true;
            }
        }

        ~WindowEventHook() {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
