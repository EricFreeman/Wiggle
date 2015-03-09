using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

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

        private NotifyIcon _ni = new NotifyIcon();

        #endregion

        #region Window

        public MainWindow()
        {
            InitializeComponent();
            InitializeNotifyIcon();

            _mouseInput = CreateMouseInput();
            _dispatcherTimer.Tick += MoveMouse; 
            _seconds = 10;
            SetInterval(null, null);
            _dispatcherTimer.Start();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _ni.Visible = false;
            _ni.Dispose();
        }

        #endregion

        #region Notify Icon

        private void InitializeNotifyIcon()
        {
            _ni.Icon = new System.Drawing.Icon("Icon.ico");
            _ni.Visible = true;
            _ni.DoubleClick += delegate { OpenWindow(); };
            _ni.Text = "Wiggle";

            var cm = new ContextMenu();
            cm.MenuItems.Add(0, new MenuItem("Open", delegate { OpenWindow(); }));
            cm.MenuItems.Add(1, new MenuItem("Exit", (sender, args) => Close()));
            _ni.ContextMenu = cm;
        }

        private void OpenWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Focus();
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