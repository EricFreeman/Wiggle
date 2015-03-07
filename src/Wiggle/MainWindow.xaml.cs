using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Wiggle
{
    public partial class MainWindow
    {
        #region Properties

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

        private int _seconds
        {
            get { return GetInteger(SecondsEdit.Text); }
            set { SecondsEdit.Text = value.ToString(); }
        }

        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            _dispatcherTimer.Tick += Wiggle; 
            _seconds = 10;
            SetInterval(null, null);
            _dispatcherTimer.Start();
        }

        #endregion

        #region Mouse Control

        private bool _moveLeft;

        private void Wiggle(object sender, EventArgs e)
        {
            MoveMouse();
        }

        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private static Win32Interop.INPUT[] _junkForMouseInput = CreateMouseInput();

        private static Win32Interop.INPUT[] CreateMouseInput()
        {
            var i = new Win32Interop.INPUT()
            {
                dwType = Win32Interop.InputType.INPUT_MOUSE,
                mkhi = new Win32Interop.MOUSEKEYBDHARDWAREINPUT()
                {
                    mi = new Win32Interop.MOUSEINPUT()
                    {
                        dx = 0,
                        dy = 0,
                        mouseData = 0,
                        dwFlags = Win32Interop.MouseEventFlags.MOVE,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };
            return new Win32Interop.INPUT[] { i };
        }

        private void MoveMouse()
        {
            Win32Interop.SendInput(1, _junkForMouseInput, Marshal.SizeOf(_junkForMouseInput[0]));
        }

        #endregion

        #region Helper

        public int GetInteger(string value)
        {
            int temp;
            int.TryParse(value, out temp);
            return temp;
        }

        public void SetInterval(object sender, EventArgs e)
        {
            _dispatcherTimer.Interval = new TimeSpan(0, 0, _seconds);
        }

        #endregion
    }
}