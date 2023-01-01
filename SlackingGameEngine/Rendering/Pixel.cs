using System.Runtime.InteropServices;

namespace SlackingGameEngine.Rendering;

/// <summary>
/// Used as a pixel on the command prompt. Has the size of an uint
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Pixel
{
    /// <summary>
    /// The character that will disblayed represented in unicode 
    /// </summary>
    [FieldOffset(0)] public Unicode Char;

    /// <summary>
    /// The color of the character disblayed, the 0b00001111 bits are forground color, and 0b11110000 is background color. All using the ConsoleColor enum.
    /// </summary>
    [FieldOffset(2)] public Color Color;


    public unsafe Pixel(uint pixel)
    {
        short* ptr = (short*)&pixel;
        Char = ptr[0];
        Color = ptr[1];
    }

    public Pixel(Pixel pixel)
    {
        Char = pixel.Char;
        Color = pixel.Color;
    }

    public Pixel(short c, Color color)
    {
        Char = c;
        Color = color;
    }

    public Pixel(short c, byte foreground, byte background)
    {
        Char = c;
        Color = (short)((int)foreground | ((int)background << 4));
    }

    public Pixel(short c, short foreground, short background)
    {
        Char = c;
        Color = (short)((int)foreground | ((int)background << 4));
    }

    public Pixel(short c, ConsoleColor foreground, ConsoleColor background)
    {
        Char = c;
        Color = (short)((int)foreground | ((int)background << 4));
    }

    public Pixel(short c, short color)
    {
        Char = c;
        Color = color;
    }
}
