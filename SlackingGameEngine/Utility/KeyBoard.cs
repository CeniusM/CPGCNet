using SlackingGameEngine.Win32Handles;

namespace SlackingGameEngine.Utility;

/// <summary>
/// Gets updated every time we update the engine. This class can say if a key is pressed or if it was just pressed or just lifted
/// </summary>
public unsafe class KeyBoard
{
    public bool IsKeyPressed(int i) => Pressed[i];
    public bool IsKeyJustPressed(int i) => JustPressed[i];
    public bool IsKeyJustReleased(int i) => JustReleased[i];

    public bool IsKeyPressed(char i) => Pressed[i];
    public bool IsKeyJustPressed(char i) => JustPressed[i];
    public bool IsKeyJustReleased(char i) => JustReleased[i];

    internal bool* BeforeUpdate;
    internal bool* JustReleased;
    internal bool* JustPressed;
    internal bool* Pressed;

    internal KeyBoard(Allocator allocator)
    {
        //BeforeUpdate = (bool*)allocator.GetUnsafeMemory(256);
        //JustReleased = (bool*)allocator.GetUnsafeMemory(256);
        //JustPressed = (bool*)allocator.GetUnsafeMemory(256);
        //Pressed = (bool*)allocator.GetUnsafeMemory(256);

        bool* ptr = (bool*)allocator.GetUnsafeMemory(256 * 4);
        BeforeUpdate = &ptr[256 * 0];
        JustReleased = &ptr[256 * 1];
        JustPressed = &ptr[256 * 2];
        Pressed = &ptr[256 * 3];
    }

    internal unsafe void Update(KeyboardHandle handle)
    {
        byte* keyStates = handle.keyStates;

        // Key keyStates into Pressed
        for (int i = 0; i < 256; i++)
            Pressed[i] = ((keyStates[i] & 0x80) == 0x80);

        // ajust BeforeUpdate, JustReleased, JustPressed
        for (int i = 0; i < 256; i++)
        {
            // Pressed down
            if (Pressed[i])
            {
                if (!BeforeUpdate[i])
                {
                    JustPressed[i] = true;
                    JustReleased[i] = false;
                }
                else
                {
                    JustPressed[i] = false;
                    JustReleased[i] = false;
                }
            }

            // Released
            else
            {
                if (BeforeUpdate[i])
                {
                    JustReleased[i] = true;
                    JustPressed[i] = false;
                }
                else
                {
                    JustReleased[i] = false;
                    JustPressed[i] = false;
                }
            }
        }
        for (int i = 0; i < 256; i++)
            BeforeUpdate[i] = Pressed[i];
    }
}
