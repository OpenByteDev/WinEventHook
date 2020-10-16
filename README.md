# WinEventHook
A managed wrapper over [SetWinEventHook](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwineventhook) and [UnhookWinEvent](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwinevent).

## Usage
Prints all events to the console:
```cs
using var hook = new WindowEventHook();
hook.WinEventProc += (hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime) => {
    Console.WriteLine(eventType);
};
hook.HookGlobal();
Console.Read();
```

Note: Your applicaiton needs a message loop to receive events.
