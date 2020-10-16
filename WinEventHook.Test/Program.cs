using System;
using System.Runtime.InteropServices;

namespace WinEventHook.Test {
    class Program {
        static void Main(string[] args) {
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
        struct MSG {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

    }
}
