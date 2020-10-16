using System;
using System.Runtime.InteropServices;

namespace WinEventHook.Test {
    public static class Program {
        public static void Main() {
            // hook logic
            using var hook = new WindowEventHook(WindowEvent.EVENT_MIN, WindowEvent.EVENT_MAX);
            hook.WinEventProc += (hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime) => {
                Console.WriteLine(eventType);
            };
            hook.HookGlobal();

            // simple message loop
            while (true) {
                var res = GetMessage(out var msg, IntPtr.Zero, 0, 0);

                if (res == 0)
                    break;

                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        internal static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        internal static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

    }
}
