using SlackingGameEngine;

namespace SlackingGameEngine.Win32Handles;

/// <summary>
/// Takes care of the api's between the program and the operating system
/// </summary>
internal class CursorHandle
{
    internal Queue<WindowsAPI.POINT> Movment = new Queue<WindowsAPI.POINT>();

    internal CursorHandle()
    {

    }

    internal void Update()
    {
        WindowsAPI.GetCursorPos(out var point);
        Movment.Enqueue(point);
    }

    internal void ShowCursor(bool Visable)
    {
        WindowsAPI.ShowCursor(Visable);
    }

    internal void SetCursorPos(int x, int y)
    {
        WindowsAPI.SetCursorPos(x, y);
    }

    internal void GetCursorPos(out int x, out int y)
    {
        WindowsAPI.GetCursorPos(out var point);
        x = point.X;
        y = point.Y;
    }

    ~CursorHandle()
    {
        ShowCursor(true);
    }
}
