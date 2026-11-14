using OpenTK.Windowing.GraphicsLibraryFramework;
using Kralt.Mathematics;

#nullable disable

namespace Kralt.Framework.InputHandling;

public static class Input
{
    internal static KeyboardState keyboard;
    internal static MouseState mouse;

    /// <summary>
    /// Screen-space position of the cursor
    /// </summary>
    public static Vector2 MousePosition { get; private set; }

    /// <summary>
    /// Returns the current movement of the mouse calculated frame-by-frame
    /// </summary>
    public static Vector2 MouseDelta => mouse.Delta;

    /// <summary>
    /// Positive when scrolling up, negative when scrolling down
    /// </summary>
    public static Vector2 MouseWheelDelta => mouse.ScrollDelta;

    /// <summary>
    /// Returns a movement vector created by the arrow keys
    /// </summary>
    public static Vector2 ArrowKeyDelta
    {
        get
        {
            float u = GetKey(Key.Up) ? 1f : 0f;
            float d = GetKey(Key.Down) ? 1f : 0f;
            float l = GetKey(Key.Left) ? 1f : 0f;
            float r = GetKey(Key.Right) ? 1f : 0f;

            return new Vector2(r - l, u - d);
        }
    }

    /// <summary>
    /// Returns a movement vector created by the WASD keys
    /// </summary>
    public static Vector2 WasdDelta
    {
        get
        {
            float u = GetKey(Key.W) ? 1f : 0f;
            float d = GetKey(Key.S) ? 1f : 0f;
            float l = GetKey(Key.A) ? 1f : 0f;
            float r = GetKey(Key.D) ? 1f : 0f;

            return new Vector2(r - l, u - d);
        }
    }

    /// <summary>
    /// Returns true for all frames that the user is holding this <see cref="Key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKey(Key key) => keyboard.IsKeyDown((Keys)key);

    /// <summary>
    /// Returns true on the frame the user starts pressing this <see cref="Key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyDown(Key key) => keyboard.IsKeyPressed((Keys)key);

    /// <summary>
    /// Returns true on the frame the user stops pressing the <see cref="Key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyUp(Key key) => keyboard.IsKeyReleased((Keys)key);

    /// <summary>
    /// Returns true for all frames that the user is holding this <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseButton"/>
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButton(MouseButton button) => mouse.IsButtonDown((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)button);

    /// <summary>
    /// Returns true on the frame that the user starts pressing this <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseButton"/>
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButtonDown(MouseButton button) => mouse.IsButtonPressed((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)button);

    /// <summary>
    /// Returns true on the frame that the user stops pressing this <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseButton"/>
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButtonUp(MouseButton button) => mouse.IsButtonReleased((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)button);
}
