using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Wiggle
{
    public partial class MainWindow
    {
        #region Properties

        private int _seconds
        {
            get { return GetInteger(SecondsEdit.Text); }
            set { SecondsEdit.Text = value.ToString(); }
        }

        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private static Win32Interop.INPUT[] _mouseInput;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            _mouseInput = CreateMouseInput();
            _dispatcherTimer.Tick += MoveMouse; 
            _seconds = 10;
            SetInterval(null, null);
            _dispatcherTimer.Start();
        }

        #endregion

        #region Mouse Control

        private static Win32Interop.INPUT[] CreateMouseInput()
        {
            var i = new Win32Interop.INPUT
            {
                dwType = Win32Interop.InputType.INPUT_MOUSE,
                mkhi = new Win32Interop.MOUSEKEYBDHARDWAREINPUT
                {
                    mi = new Win32Interop.MOUSEINPUT
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
            return new[] { i };
        }

        private void MoveMouse(object sender, EventArgs e)
        {
            Win32Interop.SendInput(1, _mouseInput, Marshal.SizeOf(_mouseInput[0]));
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