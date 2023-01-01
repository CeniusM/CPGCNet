using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SlackingGameEngine.Rendering;

namespace SlackingGameEngine.Win32Handles;


internal unsafe class CommandPromptHandle
{
    SafeFileHandle handle;

    IntPtr stdHandle;
    IntPtr stdHandleIn;

    internal CommandPromptHandle()
    {
        // Gets a console if this proces dosent allready have one
        WindowsAPI.AllocConsole();

        // Get cmd buffer
        handle = WindowsAPI.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        if (handle.IsInvalid)
            throw new Exception("Were not able to create cmd SafeFileHandle");

        stdHandle = WindowsAPI.GetStdHandle(WindowsAPI.STD_OUTPUT_HANDLE);
        stdHandleIn = WindowsAPI.GetStdHandle(WindowsAPI.STD_INPUT_HANDLE);

        WindowsAPI.SetConsoleMode(stdHandleIn, (uint)WindowsAPI.ConsoleModeFlags.ENABLE_EXTENDED_FLAGS);
        WindowsAPI.FlushConsoleInputBuffer(stdHandleIn);
    }

    internal (short X, short Y) GetFontSize()
    {
        if (stdHandle == WindowsAPI.INVALID_HANDLE_VALUE)
            throw new Exception("Could not return anything beacous of bad stdHandle");

        WindowsAPI.CONSOLE_FONT_INFO_EX info = new WindowsAPI.CONSOLE_FONT_INFO_EX();
        info.cbSize = (uint)Marshal.SizeOf(info);
        if (WindowsAPI.GetCurrentConsoleFontEx(stdHandle, false, ref info))
        {
            return (info.dwFontSize.X, info.dwFontSize.Y);
        }
        throw new IOException("");
    }

    internal void SetFontSize(short size)
    {
        string fontName;
        if (stdHandle == WindowsAPI.INVALID_HANDLE_VALUE)
            return;

        WindowsAPI.CONSOLE_FONT_INFO_EX info = new WindowsAPI.CONSOLE_FONT_INFO_EX();
        info.cbSize = (uint)Marshal.SizeOf(info);
        if (WindowsAPI.GetCurrentConsoleFontEx(stdHandle, false, ref info))
        {
            // Set console font to Lucida Console.
            WindowsAPI.CONSOLE_FONT_INFO_EX newInfo = new WindowsAPI.CONSOLE_FONT_INFO_EX();
            newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
            newInfo.FontFamily = WindowsAPI.TMPF_TRUETYPE;
            fontName = new string(info.FaceName);
            IntPtr ptr = new IntPtr(newInfo.FaceName);
            Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
            // Get some settings from current font.
            newInfo.dwFontSize = new WindowsAPI.COORD(size, size);
            newInfo.FontWeight = info.FontWeight;
            WindowsAPI.SetCurrentConsoleFontEx(stdHandle, false, newInfo);
        }
    }

    internal float GetAspectRatio()
    {
        var fontSize = GetFontSize();
        return (float)fontSize.X / (float)fontSize.Y;
    }

    internal void SetWindowSizeToBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer == 0)
            throw new NullReferenceException("Buffer have not been initialized");

        // Very very bad way of doing it
        // And please just save the fontsizes, console size, and aspect ratio in class variables
        try
        {
#pragma warning disable CA1416 // Validate platform compatibility
            Console.SetWindowSize(buffer->width, buffer->height);
            Console.SetBufferSize(buffer->width, buffer->height);
#pragma warning restore CA1416 // Validate platform compatibility
        }
        catch (Exception)
        {
            short size = (short)(GetFontSize().X - 1);
            if (size < 1)
                throw new Exception("Wasent able to fit the command prompt inside the screen with the PixelBuffer width and height");
            SetFontSize(size);
            SetWindowSizeToBuffer(buffer);
            while (GetAspectRatio() < 0.6f)
            {
                size = (short)(GetFontSize().Y - 1);
                SetFontSize(size);
                SetWindowSizeToBuffer(buffer);
                if (size < 1)
                    break;
            }
        }
    }

    internal bool RenderBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer == 0)
            throw new NullReferenceException("Buffer have not been initialized");

        Console.CursorVisible = false;

        // Stack allocated struct
        var s = new CoordRect() { Left = 0, Top = 0, Right = buffer->width, Bottom = buffer->height };
        return WindowsAPI.WriteConsoleOutputW(handle, buffer->buffer, new Coord(buffer->width, buffer->height), new Coord(0, 0), ref s);
    }



    ~CommandPromptHandle()
    {
        handle.Dispose();
    }
}