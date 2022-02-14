using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinRT;

namespace Microsoft.UI.Xaml
{
    //Note: Don't prefer extensions, remove when not required; suffix all methods with Ex.
    public static class WindowExtensions
    {
        public static void SetIconEx(this Window window, string iconName)
        {
            //Issue: https://github.com/microsoft/microsoft-ui-xaml/issues/4056
            LoadIcon(iconName, window);
        }

        public static void SetWindowSizeEx(this Window window, int width, int height)
        {
            //Issue: https://github.com/microsoft/microsoft-ui-xaml/issues/6353
            //IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            //var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            //var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            //appWindow.Resize(new Windows.Graphics.SizeInt32(1200, 720));
            //SetWindowSize(m_windowHandle, 875, 875);

            var hwnd = GetWindowHandleEx(window);
            var dpi = PInvoke.User32.GetDpiForWindow(hwnd);
            float scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            PInvoke.User32.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, width, height, PInvoke.User32.SetWindowPosFlags.SWP_NOMOVE);
        }

        public static IntPtr GetWindowHandleEx(this Window window)
        {
            var windowNative = window.As<IWindowNative>();
            return windowNative.WindowHandle;
        }

        #region helpers

        private const int IMAGE_ICON = 1;
        private const int LR_LOADFROMFILE = 0x0010;

        private static void LoadIcon(string iconName, Window window)
        {
            //Get the Window's HWND
            var hwnd = window.As<IWindowNative>().WindowHandle;
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero,
                iconName,
                PInvoke.User32.ImageType.IMAGE_ICON,
                16,
                16,
                PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);

            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        #endregion //helpers
    }
}
