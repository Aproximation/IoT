using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MirrorApp
{
    public class Display
    {
        public int WM_SYSCOMMAND = 0x0112;
        public int SC_MONITORPOWER = 0xF170; //Using the system pre-defined MSDN constants that can be used by the SendMessage() function .
        int HWND_BROADCAST = 0xffff;
        bool state = true;
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        public enum MonitorState
        {
            ON = -1,
            OFF = 2,
            STANDBY = 1
        }
        public void SetMonitorState(MonitorState state)
        {
            IntPtr xAsIntPtr = new IntPtr(HWND_BROADCAST);
            SendMessage(xAsIntPtr, WM_SYSCOMMAND, SC_MONITORPOWER, 2);//DLL function
        }
    }
}
