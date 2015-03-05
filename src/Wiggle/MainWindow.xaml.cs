using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Wiggle
{
    public partial class MainWindow
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public MainWindow()
        {
            InitializeComponent();

            Wiggle();
        }

        private void Wiggle()
        {
            var point = GetMousePosition();
            SetCursorPos((int)point.X + 1, (int)point.Y);
        }

        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
