using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;
using Kralt.Framework.UI;
using Kralt.Mathematics;

namespace Kralt.Framework;

public abstract class Application(string title, int width, int height) : 
    GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title, Vsync = VSyncMode.On })
{
    public Scene? activeScene { get; protected set; }

    public Drawable[]? activeUI { get; protected set; }

    private float viewportAspect;

    #region Application life cycle

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void GenerateApplicationResources() { }

    #endregion

    #region Initialization

    protected override void OnLoad()
    {
        base.OnLoad();

        InitializeGraphics();
        InitializeResources();
        InitializeHelperClasses();
        GenerateApplicationResources();
        UpdateViewportAspect();

        UIRenderer.Initialize();

        Start();
        InitializeActiveScene();
    }

    private void InitializeGraphics()
    {
        GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
        GL.ClearColor(Color.black);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
    }

    private void InitializeResources()
    {
        // Fallback shader (unlit pink)
        Resources.shaders.Add(
            "fallback", 
            ShaderProgram.Load(Path.Combine(AppContext.BaseDirectory, @"Graphics\Shaders"), "fallback"));

        Resources.materials.Add(
            "fallback", 
            new(Resources.shaders["fallback"]));

        // Default UI shader
        Resources.shaders.Add(
            "ui-default", 
            ShaderProgram.Load(Path.Combine(AppContext.BaseDirectory, @"Graphics\Shaders\UI"), "ui-default"));

        Resources.materials.Add(
            "ui-default", 
            new(Resources.shaders["ui-default"]));
    }

    private void InitializeHelperClasses()
    {
        Mouse.Initialize(
            e => CursorState = e switch
            {
                CursorLockMode.Locked => CursorState.Grabbed,
                CursorLockMode.Confined => CursorState.Confined,
                CursorLockMode.Hidden => CursorState.Hidden,
                _ => CursorState.Normal
            },
            () => CursorState switch
            {
                CursorState.Grabbed => CursorLockMode.Locked,
                CursorState.Confined => CursorLockMode.Confined,
                CursorState.Hidden => CursorLockMode.Hidden,
                _ => CursorLockMode.None
            }
        );
    }

    private void InitializeActiveScene()
    {
        activeScene?.Initialize();
        activeScene?.Start_Internal();
    }

    #endregion

    #region Update

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        UpdateUserInput();
        Time.UpdateTime((float)e.Time);

        activeScene?.Update_Internal();
        Update();
    }

    private void UpdateUserInput()
    {
        Input.keyboard = KeyboardState;
        Input.mouse = MouseState;
    }

    #endregion

    #region Render Loop

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        DrawScene();
        DrawUI();
        SwapBuffers();
    }

    private void DrawScene()
    {
        if (activeScene == null)
            return;
        
        Renderer.RenderFrame(activeScene, viewportAspect);
    }

    private void DrawUI()
    {
        if (activeUI == null)
            return;
        
        UIRenderer.RenderFrame(activeUI, (Vector2Int)Size);
    }

    #endregion

    #region Window Events

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        UpdateViewportAspect();
    }

    private void UpdateViewportAspect()
    {
        viewportAspect = FramebufferSize.Y > 0 ? FramebufferSize.X / (float)FramebufferSize.Y : 1f;
    }

    #endregion
}
