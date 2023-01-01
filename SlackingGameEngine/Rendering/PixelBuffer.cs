using System.Runtime.InteropServices;

namespace SlackingGameEngine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PixelBuffer
{
    internal Pixel* buffer;
    internal uint bufferSize;
    internal ushort width;
    internal ushort height;

    internal PixelBuffer(PixelBuffer* buffer)
    {
        this.width = buffer->width;
        this.height = buffer->width;
        this.buffer = buffer->buffer;
        this.bufferSize = buffer->bufferSize;
    }

    internal PixelBuffer(ushort Width, ushort Height, Pixel* buffer)
    {
        this.width = Width;
        this.height = Height;
        this.buffer = buffer;
        this.bufferSize = (uint)(Height * Width);
    }

    /// <summary>
    /// Returns a new allocated PixelBuffer
    /// </summary>
    public static PixelBuffer* GetNewPixelBuffer(ushort Width, ushort Height)
    {
        // Argument check
        //if (Width < 1 || Height< 1)
        //    throw new ArgumentException("Neither height nor width can be blow 1");

        IntPtr pixelBuffer = Marshal.AllocHGlobal(sizeof(PixelBuffer));
        IntPtr pixelArray = Marshal.AllocHGlobal(Width * Height);
        if (pixelArray == pixelBuffer)
        Console.WriteLine("HI");

        // Set buffer varibles
        PixelBuffer* buffer = (PixelBuffer*)Marshal.AllocHGlobal(sizeof(PixelBuffer));
        buffer->width = Width;
        buffer->height = Height;
        buffer->bufferSize = (uint)(Width * Height);
        buffer->buffer = (Pixel*)Marshal.AllocHGlobal((int)buffer->bufferSize * 4);
        ClearBuffer(buffer);

        return buffer;
    }

    /// <summary>
    /// Sets the Pixel Array and the varibles that comes with it to the new desired values
    /// </summary>
    public static void SetBuffer(PixelBuffer* buffer, ushort Width, ushort Height)
    {
        // Delete buffer if it is allready allocated
        DeleteBuffer(buffer);

        // Set buffer varibles
        buffer->height = Height;
        buffer->width = Width;
        buffer->bufferSize = (uint)(Height * Width);
        buffer->buffer = (Pixel*)Marshal.AllocHGlobal((int)buffer->bufferSize * sizeof(Pixel));
        ClearBuffer(buffer);
    }

    /// <summary>
    /// Deltes both the Pixel Array and the PixelBuffer struct holding the data
    /// </summary>
    public static void DeleteBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer->buffer != 0)
            Marshal.FreeHGlobal((IntPtr)buffer->buffer);
        if ((uint)buffer != 0)
            Marshal.FreeHGlobal((IntPtr)buffer);
    }

    /// <summary>
    /// Sets all the values in the Pixel Array to 0
    /// </summary>
    public static void ClearBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer->buffer == 0)
            throw new NullReferenceException("Buffer haven't been initialized");

        uint* tempBufferPtr = (uint*)buffer->buffer;
        for (int i = 0; i < buffer->bufferSize; i++)
            tempBufferPtr[i] = 0;
    }
}
