using System.Diagnostics;
using SlackingGameEngine.Win32Handles;
using SlackingGameEngine.Rendering;
using SlackingGameEngine.Utility;

namespace SlackingGameEngine;

/// <summary>
/// A Windows specific, commandpromt based game engine
/// </summary>
public unsafe class SlackingGameEngine
{
    internal CommandPromptHandle cmdHandle;
    internal KeyboardHandle keyboardHandle;
    internal CursorHandle cursorHandle;
    internal PixelBuffer* activeBuffer;
    internal Allocator allocator;
    internal KeyBoard KeyBoard;
    internal Cursor Cursor;

    public Cursor GetCursorController() => Cursor;
    public KeyBoard GetKeyBoardController() => KeyBoard;

    public SlackingGameEngine(ushort width, ushort height)
    {
        cmdHandle = new CommandPromptHandle();
        keyboardHandle = new KeyboardHandle();
        cursorHandle = new CursorHandle();

        activeBuffer = PixelBuffer.GetNewPixelBuffer(width, height);
        cmdHandle.SetWindowSizeToBuffer(activeBuffer);

        allocator = new Allocator();

        KeyBoard = new KeyBoard(allocator);
        Cursor = new Cursor();
    }

    /// <summary>
    /// True = pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyState(char key) => keyboardHandle.GetKeyState(key);

    public IntPtr GetActiveBuffer() => new IntPtr(activeBuffer);
    public void SetActiveBuffer(IntPtr* buffer) => activeBuffer = (PixelBuffer*)buffer;

    public ushort GetWidthOfBuffer() => activeBuffer->width;
    public ushort GetHeightOfBuffer() => activeBuffer->height;

    /// <summary>
    /// Return the current aspect ratio of the command prompt ( x / y )
    /// </summary>
    public float GetAspectRatio() => cmdHandle.GetAspectRatio();

    public void Start()
    {

    }

    public void Update()
    {
        keyboardHandle.Update();
        KeyBoard.Update(keyboardHandle);
        cursorHandle.Update();
        Cursor.Update(cursorHandle);
    }

    public bool ShowFPS = false;
    public float DeltaF = 0;
    public double Delta = 0;
    private Stopwatch sw = new Stopwatch();
    public void RenderBuffer()
    {
        if (ShowFPS)
            Console.Title = sw.ElapsedMilliseconds.ToString();
        cmdHandle.RenderBuffer(activeBuffer);
        Delta = sw.Elapsed.TotalMilliseconds;
        DeltaF = (float)Delta;
        sw.Restart();
    }

    ~SlackingGameEngine()
    {
        PixelBuffer.DeleteBuffer(activeBuffer);
        allocator.FreeAllPointers();
    }
}