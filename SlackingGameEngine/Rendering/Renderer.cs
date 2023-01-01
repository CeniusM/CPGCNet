using SlackingGameEngine.Win32Handles;
using System.Runtime.InteropServices;

namespace SlackingGameEngine.Rendering;

public unsafe class Renderer
{
    #region Consts

    public const ushort UZero = 0;
    public const short Zero = 0;

    /// <summary>
    /// Range 0 - 4
    /// </summary>
    public readonly int[] GRADIANTS = {
        0x20,
        0x2591,
        0x2592,
        0x2593,
        0x2588,
    };

    // Colors in byte
    public const byte BLACK = (byte)ConsoleColor.Black;
    public const byte DARKBLUE = (byte)ConsoleColor.DarkBlue;
    public const byte DARKGREEN = (byte)ConsoleColor.DarkGreen;
    public const byte DARKCYAN = (byte)ConsoleColor.DarkCyan;
    public const byte DARKRED = (byte)ConsoleColor.DarkRed;
    public const byte DARKMAGENTA = (byte)ConsoleColor.DarkMagenta;
    public const byte DARKYELLOW = (byte)ConsoleColor.DarkYellow;
    public const byte GRAY = (byte)ConsoleColor.Gray;
    public const byte DARKGRAY = (byte)ConsoleColor.DarkGray;
    public const byte BLUE = (byte)ConsoleColor.Blue;
    public const byte GREEN = (byte)ConsoleColor.Green;
    public const byte CYAN = (byte)ConsoleColor.Cyan;
    public const byte RED = (byte)ConsoleColor.Red;
    public const byte MAGENTA = (byte)ConsoleColor.Magenta;
    public const byte YELLOW = (byte)ConsoleColor.Yellow;
    public const byte WHITE = (byte)ConsoleColor.White;

    // Color masks
    public const int FOREGROUND_MASK = 0b00001111;
    public const int BACKGROUND_MASK = 0b11110000;
    #endregion

    public static void Clear(IntPtr buffer) =>
                       Clear((PixelBuffer*)buffer, new Pixel(0));
    public static void Clear(IntPtr buffer, Pixel pixel) =>
                       Clear((PixelBuffer*)buffer, pixel);
    public static void Clear(PixelBuffer* buffer, Pixel pixel)
    {
        uint bufferSize = buffer->bufferSize;
        uint* bufferPtr = (uint*)buffer->buffer;
        for (int i = 0; i < bufferSize; i++)
            bufferPtr[i] = 0;
    }

    #region RectRender
    public static void RenderRect(IntPtr buffer, ushort x, ushort y, ushort width, ushort height, Pixel pixel) =>
                       RenderRect((PixelBuffer*)buffer, x, y, width, height, pixel);
    public static void RenderRect(PixelBuffer* buffer, ushort x, ushort y, ushort width, ushort height, Pixel pixel)
    {
        int Left = x < buffer->width ? x : buffer->width;
        int Top = y < buffer->height ? y : buffer->height;
        width = x + width > buffer->width ? (ushort)(buffer->width - Left) : width;
        int Bottom = y + height > buffer->height ? buffer->height : height + Top;

        for (int i = Top; i < Bottom; i++)
        {
            Pixel* ptr = &buffer->buffer[i * buffer->width + Left];
            for (int j = 0; j < width; j++)
            {
                ptr[j] = pixel;
            }
        }
    }
    #endregion


    #region LineRender
    public static void RenderLine(IntPtr pixelBufer, int x1, int y1, int x2, int y2, int width, Pixel pixel) =>
                       RenderLine((PixelBuffer*)pixelBufer, x1, y1, x2, y2, width, pixel);
    public static void RenderLine(PixelBuffer* pixelBuffer, int x1, int y1, int x2, int y2, int width, Pixel pixel)
    {
        Pixel* buffer = pixelBuffer->buffer;
        ushort bufferWidth = pixelBuffer->width;
        ushort bufferHeight = pixelBuffer->height;

        float a = ((float)y2 - y1) / ((float)x2 - x1);

        // Vertical
        if (float.IsInfinity(a))
        {
            int start = Math.Max(0, Math.Min(y1, y2));
            int end = Math.Min(bufferHeight, Math.Max(y1, y2)) + 1;
            int x = Math.Clamp(x1, 0, bufferWidth);

            // moves the line to the left
            Pixel* line = &buffer[x];

            for (int i = start; i < end; i++)
                line[i * bufferWidth] = pixel;
        }

        // Horizantal
        else if (!float.IsNormal(a))
        {
            int start = Math.Max(0, Math.Min(x1, x2));
            int end = Math.Min(bufferWidth, Math.Max(x1, x2)) + 1;
            int y = Math.Clamp(y1, 0, bufferHeight);
            Pixel* Line = &buffer[y * bufferWidth];
            for (int i = start; i < end; i++)
                Line[i] = pixel;
        }

        // Normal
        else
        {
            //float xStart = Math.Min(x1, x2);
            //float yStart = Math.Min(y1, y2);
            //float xEnd = Math.Max(x1, x2);
            //float yEnd = Math.Max(y1, y2);

            float b = y1 - (x1 * a);

            int xStart = Math.Min(x1, x2);
            int xEnd = Math.Max(x1, x2) + 1;

            for (int x = xStart; x < xEnd; x++)
            {
                // y = ax+b
                int y = (int)(x * a + b);
                buffer[y * bufferWidth + x] = pixel;
            }
        }
    }
    #endregion

    #region TextRender

    public static void RenderText(IntPtr buffer, ushort Left, ushort Top, string text, short color)
    {
        ushort lenght = (ushort)text.Length;
        Unicode[] newText = new Unicode[lenght];

        for (ushort i = 0; i < lenght; i++)
            newText[i] = (Unicode)text[i];

        RenderText((PixelBuffer*)buffer, Left, Top, newText, color);
    }

    public static void RenderText(PixelBuffer* buffer, ushort Left, ushort Top, Unicode[] text, short color)
    {
        ushort Lenght = (ushort)text.Length;

        if (buffer->height <= Top)
            return;
        if (buffer->width < Left + Lenght)
            Lenght = (ushort)(buffer->width - Left);

        Pixel* StartOfBuffer = &buffer->buffer[Top * buffer->width + Left];
        Pixel pixel = new Pixel((Unicode)' ', color);
        for (int i = 0; i < Lenght; i++)
        {
            pixel.Char = text[i];
            StartOfBuffer[i] = pixel;
        }


    }
    #endregion

}
