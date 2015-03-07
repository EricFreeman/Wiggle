using System;
using System.Runtime.InteropServices;

namespace Wiggle
{
    class Win32Interop
    {
        [DllImport("User32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("User32.dll")]
        public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);

        public enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        };

        public enum MouseEventFlags
        {
            MOVE = 0x00000001,
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            RIGHTDOWN = 0x00000008,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            ABSOLUTE = 0x00008000,
            RIGHTUP = 0x00000010
        };

        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEventFlags dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        };

        public struct INPUT
        {
            public InputType dwType;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        };

    }
}