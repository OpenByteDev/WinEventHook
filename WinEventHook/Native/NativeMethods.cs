using System;
using System.Runtime.InteropServices;

namespace WinEventHook {
    internal static class NativeMethods {

        /// <summary>
        /// Sets an event hook function for a range of events.
        /// </summary>
        /// <param name="eventMin">Specifies the event constant for the lowest event value in the range of events that are handled by the hook function. This parameter can be set to EVENT_MIN to indicate the lowest possible event value.</param>
        /// <param name="eventMax">Specifies the event constant for the highest event value in the range of events that are handled by the hook function. This parameter can be set to EVENT_MAX to indicate the highest possible event value.</param>
        /// <param name="hmodWinEventProc">Handle to the DLL that contains the hook function at lpfnWinEventProc, if the WINEVENT_INCONTEXT flag is specified in the dwFlags parameter. If the hook function is not located in a DLL, or if the WINEVENT_OUTOFCONTEXT flag is specified, this parameter is NUL</param>
        /// <param name="lpfnWinEventProc">Pointer to the event hook function.</param>
        /// <param name="idProcess">Specifies the ID of the process from which the hook function receives events. Specify zero (0) to receive events from all processes on the current desktop.</param>
        /// <param name="idThread">Specifies the ID of the thread from which the hook function receives events. If this parameter is zero, the hook function is associated with all existing threads on the current desktop.</param>
        /// <param name="dwFlags">Flag values that specify the location of the hook function and of the events to be skipped.</param>
        /// <returns>If successful, returns an HWINEVENTHOOK value that identifies this event hook instance. Applications save this return value to use it with the UnhookWinEvent function. If unsuccessful, returns zero.</returns>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwineventhook"/>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWinEventHook(WindowEvent eventMin, WindowEvent eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc,
                                                    uint idProcess, uint idThread, WinEventHookFlags dwFlags);

        /// <summary>
        /// Removes an event hook function created by a previous call to SetWinEventHook.
        /// </summary>
        /// <param name="hWinEventHook">Handle to the event hook returned in the previous call to SetWinEventHook.</param>
        /// <returns>If successful, returns TRUE; otherwise, returns FALSE.</returns>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwinevent"/>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    }
}
