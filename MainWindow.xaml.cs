using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        // Modifiers
        private const uint MOD_NONE = 0x0000; // None
        private const uint MOD_ALT = 0x0001; // ALT
        private const uint MOD_CTRL = 0x0002; // CTRL
        private const uint MOD_SHIFT = 0x0004; // SHIFT
        private const uint MOD_WIND = 0x0008; // Windows key
        private const uint VK_CAPITAL = 0x14; // CAPS LOCK
        private const uint VK_R = 0x52; // R key

        public MainWindow()
        {
            InitializeComponent();
        }

        int counter = 0;
        private IntPtr _windowHandle;
        private HwndSource _source;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CTRL, VK_R);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VK_R)
                            {
                                counter++;
                                NumLabel.Content = counter;
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            NumLabel.Content = 0;
        }
    }
}
