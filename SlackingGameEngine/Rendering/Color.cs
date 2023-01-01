using System.Runtime.InteropServices;

namespace SlackingGameEngine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public struct Color
{
    #region Colors

    public const short Black = (short)ConsoleColor.Black;
    public const short DarkBlue = (short)ConsoleColor.DarkBlue;
    public const short DarkGreen = (short)ConsoleColor.DarkGreen;
    public const short DarkCyan = (short)ConsoleColor.DarkCyan;
    public const short DarkRed = (short)ConsoleColor.DarkRed;
    public const short DarkMagenta = (short)ConsoleColor.DarkMagenta;
    public const short DarkYellow = (short)ConsoleColor.DarkYellow;
    public const short Gray = (short)ConsoleColor.Gray;
    public const short DarkGray = (short)ConsoleColor.DarkGray;
    public const short Blue = (short)ConsoleColor.Blue;
    public const short Green = (short)ConsoleColor.Green;
    public const short Cyan = (short)ConsoleColor.Cyan;
    public const short Red = (short)ConsoleColor.Red;
    public const short Magenta = (short)ConsoleColor.Magenta;
    public const short Yellow = (short)ConsoleColor.Yellow;
    public const short White = (short)ConsoleColor.White;

    public const short FG_Black = (short)ConsoleColor.Black;
    public const short FG_DarkBlue = (short)ConsoleColor.DarkBlue;
    public const short FG_DarkGreen = (short)ConsoleColor.DarkGreen;
    public const short FG_DarkCyan = (short)ConsoleColor.DarkCyan;
    public const short FG_DarkRed = (short)ConsoleColor.DarkRed;
    public const short FG_DarkMagenta = (short)ConsoleColor.DarkMagenta;
    public const short FG_DarkYellow = (short)ConsoleColor.DarkYellow;
    public const short FG_Gray = (short)ConsoleColor.Gray;
    public const short FG_DarkGray = (short)ConsoleColor.DarkGray;
    public const short FG_Blue = (short)ConsoleColor.Blue;
    public const short FG_Green = (short)ConsoleColor.Green;
    public const short FG_Cyan = (short)ConsoleColor.Cyan;
    public const short FG_Red = (short)ConsoleColor.Red;
    public const short FG_Magenta = (short)ConsoleColor.Magenta;
    public const short FG_Yellow = (short)ConsoleColor.Yellow;
    public const short FG_White = (short)ConsoleColor.White;

    public const short BG_Black = (int)ConsoleColor.Black << 4;
    public const short BG_DarkBlue = (int)ConsoleColor.DarkBlue << 4;
    public const short BG_DarkGreen = (int)ConsoleColor.DarkGreen << 4;
    public const short BG_DarkCyan = (int)ConsoleColor.DarkCyan << 4;
    public const short BG_DarkRed = (int)ConsoleColor.DarkRed << 4;
    public const short BG_DarkMagenta = (int)ConsoleColor.DarkMagenta << 4;
    public const short BG_DarkYellow = (int)ConsoleColor.DarkYellow << 4;
    public const short BG_Gray = (int)ConsoleColor.Gray << 4;
    public const short BG_DarkGray = (int)ConsoleColor.DarkGray << 4;
    public const short BG_Blue = (int)ConsoleColor.Blue << 4;
    public const short BG_Green = (int)ConsoleColor.Green << 4;
    public const short BG_Cyan = (int)ConsoleColor.Cyan << 4;
    public const short BG_Red = (int)ConsoleColor.Red << 4;
    public const short BG_Magenta = (int)ConsoleColor.Magenta << 4;
    public const short BG_Yellow = (int)ConsoleColor.Yellow << 4;
    public const short BG_White = (int)ConsoleColor.White << 4;

    #endregion

    public short Value;

    public Color(byte foreground, byte background)
    {
        Value = (short)((int)foreground | ((int)background << 4));
    }

    public Color(short foreground, short background)
    {
        Value = (short)((int)foreground | ((int)background << 4));
    }

    public Color(ConsoleColor foreground, ConsoleColor background)
    {
        Value = (short)((int)foreground | ((int)background << 4));
    }

    public Color(short color)
    {
        Value = color;
    }

    public Color(int color)
    {
        Value = (short)color;
    }

    public static implicit operator short(Color color) => color.Value;
    public static implicit operator Color(short color) => new Color(color);

    #region Static
    public static short GetColor(ConsoleColor foreGround, ConsoleColor backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }

    public static short GetColor(byte foreGround, byte backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }

    public static short GetColor(short foreGround, short backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }
    #endregion
}
