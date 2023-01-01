using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SlackingGameEngine.Rendering;
using HANDLE = System.IntPtr;
using DWORD = System.UInt32;

namespace SlackingGameEngine;

internal unsafe class WindowsAPI
{
    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern SafeFileHandle CreateFile(
      string fileName,
      [MarshalAs(UnmanagedType.U4)] uint fileAccess,
      [MarshalAs(UnmanagedType.U4)] uint fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] int flags,
      IntPtr template);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WriteConsoleOutputW(
      SafeFileHandle hConsoleOutput,
      Pixel* lpBuffer,
      Coord dwBufferSize,
      Coord dwBufferCoord,
      ref CoordRect lpWriteRegion);


    internal const int STD_OUTPUT_HANDLE = -11;
    internal const int STD_INPUT_HANDLE = -10;
    internal const int TMPF_TRUETYPE = 4;
    internal const int LF_FACESIZE = 32;
    internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool GetCurrentConsoleFontEx(
           IntPtr consoleOutput,
           bool maximumWindow,
           ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetCurrentConsoleFontEx(
           IntPtr consoleOutput,
           bool maximumWindow,
           CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

    [StructLayout(LayoutKind.Sequential)]
    internal struct COORD
    {
        internal short X;
        internal short Y;

        internal COORD(short x, short y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CONSOLE_FONT_INFO_EX
    {
        internal uint cbSize;
        internal uint nFont;
        internal COORD dwFontSize;
        internal int FontFamily;
        internal int FontWeight;
        internal fixed char FaceName[LF_FACESIZE];
    }

    [DllImport("user32.dll")]
    internal static extern int ShowCursor(bool bShow);

    [DllImport("user32.dll")]
    internal static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(out POINT point);

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public Int32 X;
        public Int32 Y;
    }


    internal enum ConsoleModeFlags : uint
    {
        ENABLE_ECHO_INPUT = 0x0004,
        ENABLE_INSERT_MODE = 0x0020,
        ENABLE_LINE_INPUT = 0x0002,
        ENABLE_MOUSE_INPUT = 0x0010,
        ENABLE_PROCESSED_INPUT = 0x0001,
        ENABLE_QUICK_EDIT_MODE = 0x0040,
        ENABLE_WINDOW_INPUT = 0x0008,
        ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,
        ENABLE_EXTENDED_FLAGS = 0x0080
    }

    [DllImport("Kernel32.dll")]
    internal static extern bool SetConsoleMode(
        HANDLE hConsoleHandle,
        DWORD dwMode
        );

    [DllImport("Kernel32.dll")]
    internal static extern bool FlushConsoleInputBuffer(
      HANDLE hConsoleInput
    );

    [DllImport("Kernel32.dll")]
    internal static extern bool AllocConsole();
}
