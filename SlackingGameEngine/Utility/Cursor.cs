using SlackingGameEngine.Win32Handles;
using SlackingGameEngine;

namespace SlackingGameEngine.Utility;

public class Cursor
{
    public struct Info
    {
        /// <summary>
        /// The pos for the given update
        /// </summary>
        public int x, y;

        /// <summary>
        /// The amount the cursor have moved since last update
        /// </summary>
        public int xOffset, yOffset;
    }

    internal bool CursorShown = true;
    internal int x = 0;
    internal int y = 0;
    Queue<Info> queue = new Queue<Info>();

    internal int nextX = -1;
    internal int nextY = -1;
    /// <summary>
    /// Return false if Cursor is loced, else set cursor to x, y next update. This will not effeckt the xOffset and yOffset
    /// </summary>
    public bool SetCursor(int x, int y)
    {
        if (LockCursor)
            return false;
        else
        {
            nextX = x;
            nextY = y;
            return true;
        }
    }

    public bool ShowCursor_NotImplemented = true;

    /// <summary>
    /// Locks the cursor in the middle of the screen
    /// </summary>
    public bool LockCursor = false;

    internal Cursor()
    {

    }

    /// <summary>
    /// Set LockedCursor to false, ShowCursor to true
    /// </summary>
    public void Reset()
    {
        LockCursor = false;
        ShowCursor_NotImplemented = false;
    }

    /// <summary>
    /// Will return true if there are more elements left to return
    /// </summary>
    public bool TryGetNextUpdate(out Info info)
    {
        if (queue.Count > 0)
        {
            info = queue.Dequeue();
            return true;
        }
        else
        {
            info = new();
            return false;
        }
    }

    /// <summary>
    /// Returns false if cursor is locked, else return true and coords
    /// </summary>
    public bool TryGetCursorPos(out int x, out int y)
    {
        x = this.x;
        y = this.y;

        if (LockCursor)
            return false;

        return true;
    }

    internal void Update(CursorHandle handle)
    {
        //if (ShowCursor && !CursorShown)
        //{
        //    handle.ShowCursor(true);
        //    CursorShown = true;
        //}
        //else if (!ShowCursor && CursorShown)
        //{
        //    handle.ShowCursor(false);
        //    CursorShown = false;
        //}

        //handle.ShowCursor(ShowCursor);

        //while (handle.Movment.Count > 0)
        //{
        var point = handle.Movment.Dequeue();

        Info info = new Info();

        info.xOffset = point.X - x;
        info.yOffset = point.Y - y;
        info.x = point.X;
        info.y = point.Y;

        if (!LockCursor)
        {
            x = point.X;
            y = point.Y;
        }
        else
        {
            x = 1920 >> 1;
            y = 1080 >> 1;
        }

        queue.Enqueue(info);
        //}

        // middle of a 1080p monitor screen
        if (LockCursor)
            handle.SetCursorPos(1920 >> 1, 1080 >> 1);
        else
        {
            if (nextX != -1)
            {
                handle.SetCursorPos(nextX, nextY);
                nextX = -1;
            }
        }
    }
}
