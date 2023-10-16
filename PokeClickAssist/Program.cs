using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Point mousePosition = new Point();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Press Enter to Capture Cursor Position...");
                mousePosition = detectMode();

                Console.Clear();
                Console.WriteLine("Clicking. Press Space to Stop...");
                clickMode(mousePosition.X, mousePosition.Y);

                if (checkEscape())
                    return;
            }
        }

        static Point detectMode()
        {
            
            while (true)
            {
                Point defPnt = new Point();

                GetCursorPos(ref defPnt);

                if (checkEnter() || checkEscape())
                {
                    return defPnt;
                }
            }
        }

        static void clickMode(int x, int y)
        {
            while (true)
            {
                mouseClick(x, y);
                Thread.Sleep(100);
                if (checkSpace() || checkEscape())
                {
                    return;
                }
            }
        }

        static bool checkEscape()
        {
            if ((GetAsyncKeyState(27) & 0x8000) > 0)
            {
                return true;
            }

            return false;
        }

        static bool checkSpace()
        {
            if ((GetAsyncKeyState(32) & 0x8000) > 0)
            {
                return true;
            }

            return false;
        }

        static bool checkEnter()
        {
            if ((GetAsyncKeyState(13) & 0x8000) > 0)
            {
                return true;
            }

            return false;
        }

        static void mouseClick(int x, int y)
        {
            moveCursor(x, y);
            sendMouseDown();
            Thread.Sleep(10);
            sendMouseUp();
        }

        static void sendMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        static void sendMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        static void moveCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

        // We need to use unmanaged code
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }
}
