# WinEventHook

![nuget badge](https://badgen.net/nuget/v/wineventhook)
[![Unlicense](https://img.shields.io/github/license/OpenByteDev/WinEventHook)](./LICENSE)

A managed wrapper over [SetWinEventHook](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwineventhook) and [UnhookWinEvent](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwinevent).


## Installation

To install with NuGet use the following command in the Packet Manager Console:
```
Install-Package WinEventHook
```

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

Note: Your application needs a message loop to receive events.
