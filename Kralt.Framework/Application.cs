using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;

#nullable disable

namespace Kralt.Framework;

public abstract class Application(string title, int width, int height) : 
    GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title, Vsync = VSyncMode.On })
{
    public int activeSceneIndex { get; private set; } = 0;

    public Scene[] scenes = [];

    public Scene activeScene => scenes[activeSceneIndex];

    private float viewportAspect;
    
    protected virtual void Start()
    {
        activeScene.Start();
    }

    protected virtual void Update()
    {
        activeScene.Update();
    }

    protected virtual void GenerateApplicationResources() { }

    public void Run(int index)
    {
        activeSceneIndex = index;
        Run();
    }

    // TODO: Clean up currently active scene then safely switch to the new one
    public void LoadScene(int index)
    {
        activeSceneIndex = index;
        scenes[index].Initialize();
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        Resources.shaders.Add("fallback", ShaderProgram.Load(Path.Combine(AppContext.BaseDirectory, "Graphics", "Shaders"), "Fallback"));
        Resources.materials.Add("fallback", new Material(Resources.shaders["fallback"]));

        GenerateApplicationResources();
        InitializeHelperClasses();

        viewportAspect = Size.X / (float)Size.Y;
        GL.ClearColor(0.5294f, 0.8078f, 0.9216f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        if(scenes.Length != 0)
        {
            LoadScene(activeSceneIndex);
            Start();
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        Input.keyboard = KeyboardState;
        Input.mouse = MouseState;
        Time.UpdateTime((float)e.Time);

        Update();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        Renderer.RenderFrame(scenes[activeSceneIndex], viewportAspect);
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
        viewportAspect = Size.X / (float)Size.Y;
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
}
