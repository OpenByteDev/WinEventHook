namespace WinEventHook {
    public enum WindowEvent : uint {
        /// <summary>
        /// The lowest possible event value.
        /// </summary>
        EVENT_MIN = 0x00000001,

        /// <summary>
        /// The lowest system event value.
        /// </summary>
        EVENT_SYSTEM_START = 0x0001,
        /// <summary>
        /// A sound has been played.
        /// The system sends this event when a system sound, such as one for a menu, is played even if no sound is audible (for example, due to the lack of a sound file or a sound card).
        /// Servers send this event whenever a custom UI element generates a sound.
        /// </summary>
        EVENT_SYSTEM_SOUND = 0x0001,
        /// <summary>
        /// An alert has been generated.
        /// </summary>
        EVENT_SYSTEM_ALERT = 0x0002,
        /// <summary>
        /// The foreground window has changed.
        /// The system sends this event even if the foreground window has changed to another window in the same thread.
        /// </summary>
        EVENT_SYSTEM_FOREGROUND = 0x0003,
        /// <summary>
        /// A menu item on the menu bar has been selected.
        /// The system sends this event for standard menus, which are identified by HMENU, created using menu-template resources or Win32 menu API elements.
        /// Servers send this event for custom menus, which are user interface elements that function as menus but are not created in the standard way.
        /// For this event, the WinEventProc callback function's hwnd, idObject, and idChild parameters refer to the control that contains the menu bar or the control that activates the context menu.
        /// The hwnd parameter is the handle to the window related to the event. The idObject parameter is OBJID_MENU or OBJID_SYSMENU for a menu, or OBJID_WINDOW for a pop-up menu.
        /// The idChild parameter is CHILDID_SELF.
        /// The system triggers more than one EVENT_SYSTEM_MENUSTART event that does not always correspond with the EVENT_SYSTEM_MENUEND event.
        /// </summary>
        EVENT_SYSTEM_MENUSTART = 0x0004,
        /// <summary>
        /// A menu from the menu bar has been closed.
        /// The system sends this event for standard menus; servers send it for custom menus.
        /// For this event, the WinEventProc callback function's hwnd, idObject, and idChild parameters refer to the control that contains the menu bar or the control that activates the context menu.
        /// The hwnd parameter is the handle to the window that is related to the event.
        /// The idObject parameter is OBJID_MENU or OBJID_SYSMENU for a menu, or OBJID_WINDOW for a pop-up menu.
        /// The idChild parameter is CHILDID_SELF.
        /// </summary>
        EVENT_SYSTEM_MENUEND = 0x0005,
        /// <summary>
        /// A pop-up menu has been displayed.
        /// The system sends this event for standard menus, which are identified by HMENU, and are created using menu-template resources or Win32 menu functions.
        /// </summary>
        EVENT_SYSTEM_MENUPOPUPSTART = 0x0006,
        /// <summary>
        /// A pop-up menu has been closed. The system sends this event for standard menus; servers send it for custom menus.
        /// When a pop-up menu is closed, the client receives this message, and then the EVENT_SYSTEM_MENUEND event.
        /// This event is not sent consistently by the system.
        /// </summary>
        EVENT_SYSTEM_MENUPOPUPEND = 0x0007,
        /// <summary>
        /// A window has received mouse capture.
        /// This event is sent by the system, never by servers.
        /// </summary>
        EVENT_SYSTEM_CAPTURESTART = 0x0008,
        /// <summary>
        /// A window has lost mouse capture.
        /// This event is sent by the system, never by servers.
        /// </summary>
        EVENT_SYSTEM_CAPTUREEND = 0x0009,
        /// <summary>
        /// A window is being moved or resized.
        /// </summary>
        EVENT_SYSTEM_MOVESIZESTART = 0x000A,
        /// <summary>
        /// The movement or resizing of a window has finished.
        /// </summary>
        EVENT_SYSTEM_MOVESIZEEND = 0x000B,
        /// <summary>
        /// A window has entered context-sensitive Help mode.
        /// This event is not sent consistently by the system.
        /// </summary>
        EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C,
        /// <summary>
        /// A window has exited context-sensitive Help mode.
        /// This event is not sent consistently by the system.
        /// </summary>
        EVENT_SYSTEM_CONTEXTHELPEND = 0x000D,
        /// <summary>
        /// An application is about to enter drag-and-drop mode.
        /// Applications that support drag-and-drop operations must send this event because the system does not send it.
        /// </summary>
        EVENT_SYSTEM_DRAGDROPSTART = 0x000E,
        /// <summary>
        /// An application is about to exit drag-and-drop mode.
        /// Applications that support drag-and-drop operations must send this event; the system does not send this event.
        /// </summary>
        EVENT_SYSTEM_DRAGDROPEND = 0x000F,
        /// <summary>
        /// A dialog box has been displayed.
        /// The system sends this event for standard dialog boxes, which are created using resource templates or Win32 dialog box functions.
        /// Servers send this event for custom dialog boxes, which are windows that function as dialog boxes but are not created in the standard way.
        /// This event is not sent consistently by the system.
        /// </summary>
        EVENT_SYSTEM_DIALOGSTART = 0x0010,
        /// <summary>
        /// An application is about to exit drag-and-drop mode.
        /// Applications that support drag-and-drop operations must send this event; the system does not send this event.
        /// </summary>
        EVENT_SYSTEM_DIALOGEND = 0x0011,
        /// <summary>
        /// Scrolling has started on a scroll bar.
        /// The system sends this event for standard scroll bar controls and for scroll bars attached to a window.
        /// Servers send this event for custom scroll bars, which are user interface elements that function as scroll bars but are not created in the standard way.
        /// The idObject parameter that is sent to the WinEventProc callback function is OBJID_HSCROLL for horizontal scrolls bars, and OBJID_VSCROLL for vertical scroll bars.
        /// </summary>
        EVENT_SYSTEM_SCROLLINGSTART = 0x0012,
        /// <summary>
        /// Scrolling has ended on a scroll bar.
        /// This event is sent by the system for standard scroll bar controls and for scroll bars that are attached to a window.
        /// Servers send this event for custom scroll bars, which are user interface elements that function as scroll bars but are not created in the standard way.
        /// The idObject parameter that is sent to the WinEventProc callback function is OBJID_HSCROLL for horizontal scroll bar, and OBJID_VSCROLL for vertical scroll bars.
        /// </summary>
        EVENT_SYSTEM_SCROLLINGEND = 0x0013,
        /// <summary>
        /// The user has pressed ALT+TAB, which activates the switch window.
        /// This event is sent by the system, never by servers.
        /// The hwnd parameter of the WinEventProc callback function identifies the window to which the user is switching.
        /// If only one application is running when the user presses ALT+TAB, the system sends an EVENT_SYSTEM_SWITCHEND event without a corresponding EVENT_SYSTEM_SWITCHSTART event.
        /// </summary>
        EVENT_SYSTEM_SWITCHSTART = 0x0014,
        /// <summary>
        /// The user has released ALT+TAB.
        /// This event is sent by the system, never by servers.
        /// The hwnd parameter of the WinEventProc callback function identifies the window to which the user has switched.
        /// If only one application is running when the user presses ALT+TAB, the system sends this event without a corresponding EVENT_SYSTEM_SWITCHSTART event.
        /// </summary>
        EVENT_SYSTEM_SWITCHEND = 0x0015,
        /// <summary>
        /// A window object is about to be minimized.
        /// </summary>
        EVENT_SYSTEM_MINIMIZESTART = 0x0016,
        /// <summary>
        /// A window object is about to be restored.
        /// </summary>
        EVENT_SYSTEM_MINIMIZEEND = 0x0017,
        /// <summary>
        /// The active desktop has been switched.
        /// </summary>
        EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,
        /// <summary>
        /// The highest system event value.
        /// </summary>
        EVENT_SYSTEM_END = 0x00FF,

        /// <summary>
        /// The lowest event value reserved for OEMs.
        /// </summary>
        EVENT_OEM_DEFINED_START = 0x0101,
        /// <summary>
        /// The highest event value reserved for OEMs.
        /// </summary>
        EVENT_OEM_DEFINED_END = 0x01FF,

        EVENT_CONSOLE_START = 0x4001,
        EVENT_CONSOLE_CARET = 0x4001,
        EVENT_CONSOLE_UPDATE_REGION = 0x4002,
        EVENT_CONSOLE_UPDATE_SIMPLE = 0x4003,
        EVENT_CONSOLE_UPDATE_SCROLL = 0x4004,
        EVENT_CONSOLE_LAYOUT = 0x4005,
        EVENT_CONSOLE_START_APPLICATION = 0x4006,
        EVENT_CONSOLE_END_APPLICATION = 0x4007,
        EVENT_CONSOLE_END = 0x40FF,

        /// <summary>
        /// The lowest event value reserved for UI Automation event identifiers.
        /// </summary>
        EVENT_UIA_EVENTID_START = 0x4E00,
        /// <summary>
        /// The highest event value reserved for UI Automation event identifiers.
        /// </summary>
        EVENT_UIA_EVENTID_END = 0x4EFF,

        /// <summary>
        /// The lowest event value reserved for UI Automation property-changed event identifiers.
        /// </summary>
        EVENT_UIA_PROPID_START = 0x7500,
        /// <summary>
        /// The highest event value reserved for UI Automation property-changed event identifiers.
        /// </summary>
        EVENT_UIA_PROPID_END = 0x75FF,

        /// <summary>
        /// The lowest object event value.
        /// </summary>
        EVENT_OBJECT_START = 0x8000,
        /// <summary>
        /// An object has been created.
        /// The system sends this event for the following user interface elements:
        /// caret, header control, list-view control, tab control, toolbar control, tree view control, and window object.
        /// </summary>
        EVENT_OBJECT_CREATE = 0x8000,
        /// <summary>
        /// n object has been destroyed.
        /// The system sends this event for the following user interface elements:
        /// caret, header control, list-view control, tab control, toolbar control, tree view control, and window object.
        /// </summary>
        EVENT_OBJECT_DESTROY = 0x8001,
        /// <summary>
        /// A hidden object is shown.
        /// The system sends this event for the following user interface elements: caret, cursor, and window object.
        /// </summary>
        EVENT_OBJECT_SHOW = 0x8002,
        /// <summary>
        /// An object is hidden.
        /// The system sends this event for the following user interface elements: caret and cursor.
        /// </summary>
        EVENT_OBJECT_HIDE = 0x8003,
        EVENT_OBJECT_REORDER = 0x8004,
        /// <summary>
        /// An object has received the keyboard focus.
        /// The system sends this event for the following user interface elements:
        /// list-view control, menu bar, pop-up menu, switch window, tab control, tree view control, and window object.
        /// </summary>
        EVENT_OBJECT_FOCUS = 0x8005,
        /// <summary>
        /// The selection within a container object has changed.
        /// The system sends this event for the following user interface elements:
        /// list-view control, tab control, tree view control, and window object.
        /// </summary>
        EVENT_OBJECT_SELECTION = 0x8006,
        /// <summary>
        /// A child within a container object has been added to an existing selection.
        /// The system sends this event for the following user interface elements:
        /// list box, list-view control, and tree view control.
        /// </summary>
        EVENT_OBJECT_SELECTIONADD = 0x8007,
        /// <summary>
        /// An item within a container object has been removed from the selection.
        /// The system sends this event for the following user interface elements:
        /// list box, list-view control, and tree view control.
        /// </summary>
        EVENT_OBJECT_SELECTIONREMOVE = 0x8008,
        /// <summary>
        /// Numerous selection changes have occurred within a container object.
        /// The system sends this event for list boxes.
        /// </summary>
        EVENT_OBJECT_SELECTIONWITHIN = 0x8009,
        /// <summary>
        /// An object's state has changed.
        /// The system sends this event for the following user interface elements:
        /// check box, combo box, header control, push button, radio button, scroll bar, toolbar control, tree view control, up-down control, and window object.
        /// </summary>
        EVENT_OBJECT_STATECHANGE = 0x800A,
        /// <summary>
        /// An object has changed location, shape, or size.
        /// The system sends this event for the following user interface elements:
        /// caret and window objects.
        /// </summary>
        EVENT_OBJECT_LOCATIONCHANGE = 0x800B,
        /// <summary>
        /// An object's Name property has changed.
        /// The system sends this event for the following user interface elements: check box, cursor, list-view control, push button, radio button, status bar control, tree view control, and window object.
        /// </summary>
        EVENT_OBJECT_NAMECHANGE = 0x800C,
        /// <summary>
        /// An object's Description property has changed.
        /// </summary>
        EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D,
        /// <summary>
        /// An object's Value property has changed.
        /// The system sends this event for the user interface elements that include the scroll bar and the following controls:
        /// edit, header, hot key, progress bar, slider, and up-down.
        /// </summary>
        EVENT_OBJECT_VALUECHANGE = 0x800E,
        /// <summary>
        /// An object has a new parent object.
        /// </summary>
        EVENT_OBJECT_PARENTCHANGE = 0x800F,
        /// <summary>
        /// An object's Help property has changed.
        /// </summary>
        EVENT_OBJECT_HELPCHANGE = 0x8010,
        /// <summary>
        /// An object's DefaultAction property has changed. The system sends this event for dialog boxes.
        /// </summary>
        EVENT_OBJECT_DEFACTIONCHANGE = 0x8011,
        /// <summary>
        /// An object's KeyboardShortcut property has changed.
        /// </summary>
        EVENT_OBJECT_ACCELERATORCHANGE = 0x8012,
        /// <summary>
        /// An object has been invoked; for example, the user has clicked a button.
        /// This event is supported by common controls and is used by UI Automation.
        /// </summary>
        EVENT_OBJECT_INVOKED = 0x8013,
        /// <summary>
        /// An object's text selection has changed.
        /// This event is supported by common controls and is used by UI Automation.
        /// </summary>
        EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014,
        /// <summary>
        /// A window object's scrolling has ended.
        /// Unlike EVENT_SYSTEM_SCROLLEND, this event is associated with the scrolling window.
        /// Whether the scrolling is horizontal or vertical scrolling, this event should be sent whenever the scroll action is completed.
        /// </summary>
        EVENT_OBJECT_CONTENTSCROLLED = 0x8015,
        /// <summary>
        /// A preview rectangle is being displayed.
        /// </summary>
        EVENT_SYSTEM_ARRANGMENTPREVIEW = 0x8016,
        /// <summary>
        /// Sent when a window is cloaked.
        /// A cloaked window still exists, but is invisible to the user.
        /// </summary>
        EVENT_OBJECT_CLOAKED = 0x8017,
        /// <summary>
        /// Sent when a window is uncloaked.
        /// A cloaked window still exists, but is invisible to the user.
        /// </summary>
        EVENT_OBJECT_UNCLOAKED = 0x8018,
        /// <summary>
        /// An object that is part of a live region has changed.
        /// A live region is an area of an application that changes frequently and/or asynchronously.
        /// </summary>
        EVENT_OBJECT_LIVEREGIONCHANGED = 0x8019,
        /// <summary>
        /// A window that hosts other accessible objects has changed the hosted objects.
        /// A client might need to query the host window to discover the new hosted objects, especially if the client has been monitoring events from the window.
        /// A hosted object is an object from an accessibility framework (MSAA or UI Automation) that is different from that of the host.
        /// Changes in hosted objects that are from the same framework as the host should be handed with the structural change events, such as EVENT_OBJECT_CREATE for MSAA.
        /// </summary>
        EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED = 0x8020,
        /// <summary>
        /// The user started to drag an element.
        /// </summary>
        EVENT_OBJECT_DRAGSTART = 0x8021,
        /// <summary>
        /// The user has ended a drag operation before dropping the dragged element on a drop target.
        /// </summary>
        EVENT_OBJECT_DRAGCANCEL = 0x8022,
        /// <summary>
        /// The user dropped an element on a drop target.
        /// </summary>
        EVENT_OBJECT_DRAGCOMPLETE = 0x8023,
        /// <summary>
        /// The user dragged an element into a drop target's boundary.
        /// </summary>
        EVENT_OBJECT_DRAGENTER = 0x8024,
        /// <summary>
        /// The user dragged an element out of a drop target's boundary.
        /// </summary>
        EVENT_OBJECT_DRAGLEAVE = 0x8025,
        /// <summary>
        /// The user dropped an element on a drop target.
        /// </summary>
        EVENT_OBJECT_DRAGDROPPED = 0x8026,
        /// <summary>
        /// An IME window has become visible.
        /// </summary>
        EVENT_OBJECT_IME_SHOW = 0x8027,
        /// <summary>
        /// An IME window has become hidden.
        /// </summary>
        EVENT_OBJECT_IME_HIDE = 0x8028,
        /// <summary>
        /// The size or position of an IME window has changed.
        /// </summary>
        EVENT_OBJECT_IME_CHANGE = 0x8029,
        /// <summary>
        /// The conversion target within an IME composition has changed.
        /// The conversion target is the subset of the IME composition which is actively selected as the target for user-initiated conversions.
        /// </summary>
        EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED = 0x8030,

        /// <summary>
        /// The highest object event value.
        /// </summary>
        EVENT_OBJECT_END = 0x80FF,

        /// <summary>
        /// The lowest event value reserved for custom events allocated at runtime.
        /// </summary>
        EVENT_ATOM_START = 0xC000,

        /// <summary>
        /// The lowest event value specified by the Accessibility Interoperability Alliance (AIA) for use across the industry.
        /// </summary>
        EVENT_AIA_START = 0xA000,
        /// <summary>
        /// The highest event value specified by the Accessibility Interoperability Alliance (AIA) for use across the industry.
        /// </summary>
        EVENT_AIA_END = 0xAFFF,

        /// <summary>
        /// The highest event value reserved for custom events allocated at runtime.
        /// </summary>
        EVENT_ATOM_END = 0xFFFF,

        /// <summary>
        /// The highest possible event value.
        /// </summary>
        EVENT_MAX = 0x7FFFFFFF,
    }
}
