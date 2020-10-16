namespace WinEventHook {
    /// <summary>
    /// This enumeration lists all kinds of accessible objects that can
    /// be directly assigned to a window.
    /// </summary>
    public enum AccessibleObjectID : uint {
        /// <summary>
        /// The window itself rather than a child object.
        /// </summary>
        OBJID_WINDOW = 0x00000000,

        /// <summary>
        /// The window's system menu.
        /// </summary>
        OBJID_SYSMENU = 0xFFFFFFFF,

        /// <summary>
        /// The window's title bar.
        /// </summary>
        OBJID_TITLEBAR = 0xFFFFFFFE,

        /// <summary>
        /// The window's menu bar.
        /// </summary>
        OBJID_MENU = 0xFFFFFFFD,

        /// <summary>
        /// The window's client area.
        /// </summary>
        OBJID_CLIENT = 0xFFFFFFFC,

        /// <summary>
        /// The window's vertical scroll bar.
        /// </summary>
        OBJID_VSCROLL = 0xFFFFFFFB,

        /// <summary>
        /// The window's horizontal scroll bar.
        /// </summary>
        OBJID_HSCROLL = 0xFFFFFFFA,

        /// <summary>
        /// The window's size grip: an optional frame component located at the lower-right corner of the window frame.
        /// </summary>
        OBJID_SIZEGRIP = 0xFFFFFFF9,

        /// <summary>
        /// The text insertion bar (caret) in the window.
        /// </summary>
        OBJID_CARET = 0xFFFFFFF8,

        /// <summary>
        /// The mouse pointer. There is only one mouse pointer in the system, and it is not a child of any window.
        /// </summary>
        OBJID_CURSOR = 0xFFFFFFF7,

        /// <summary>
        /// An alert that is associated with a window or an application.
        /// </summary>
        OBJID_ALERT = 0xFFFFFFF6,

        /// <summary>
        /// A sound object. Sound objects do not have screen locations or children, but they do have name and state attributes. They are children of the application that is playing the sound.
        /// </summary>
        OBJID_SOUND = 0xFFFFFFF5
    }
}
