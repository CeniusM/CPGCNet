using System.Runtime.InteropServices;

namespace SlackingGameEngine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public struct Unicode
{
    #region Characters

    //  gradiants
    /*
    █ full			= UTF-16 (hex) 0x2588 = alt 219
    ▓ sort of full	= UTF-16 (hex) 0x2593 = alt 178
    ▒ half			= UTF-16 (hex) 0x2592 = alt 177
    ░ low			= UTF-16 (hex) 0x2591 = alt 176
   ' ' empty		= UTF-16 (hex) 0x20   = spacebar
    */
    public const short FULL = 0x2588;
    public const short HIGH = 0x2593;
    public const short HALF = 0x2592;
    public const short LOW = 0x2591;
    public const short EMPTY = 0x20;

    #endregion

    public short Value;

    public Unicode()
    {
        Value = 0;
    }

    public Unicode(char c)
    {
        Value = (short)c;
    }

    public Unicode(short c)
    {
        Value = (short)c;
    }

    public Unicode(ushort c)
    {
        Value = (short)c;
    }

    public Unicode(int c)
    {
        Value = (short)c;
    }

    public Unicode(uint c)
    {
        Value = (short)c;
    }

    public Unicode(Unicode c)
    {
        Value = c.Value;
    }


    public static implicit operator short(Unicode color) => color.Value;
    public static implicit operator Unicode(short color) => new Unicode(color);

    public static implicit operator char(Unicode color) => (char)color.Value;
    public static implicit operator Unicode(char color) => new Unicode(color);
}
