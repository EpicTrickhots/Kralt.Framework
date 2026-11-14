using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Kralt.Framework.Graphics.Rendering;
using Kralt.Framework.Graphics;
using Kralt.Mathematics;
using Kralt.Framework.Screens;
using Kralt.Framework.Resources;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;

#nullable disable

namespace Kralt.Framework;

public abstract class Application(string title, int width, int height) : 
    GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title , Vsync = VSyncMode.On })
{
    public Screen activeScreen;
    
    public static ResourceStore resources;
    
    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void GenerateApplicationResources() { }

    protected override void OnLoad()
    {
        base.OnLoad();

        resources = new ResourceStore
        {
            fallbackShader = ShaderProgram.Compile(Path.Combine(AppContext.BaseDirectory, "Graphics", "Shaders"), "Fallback")
        };

        GenerateApplicationResources();
        InitializeHelperClasses();

        GL.ClearColor((System.Drawing.Color)Color.black);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        Start();
        activeScreen.Initialize();
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

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        activeScreen.Traverse(this);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    public void Draw(Mesh mesh, ShaderProgram shader, Matrix4x4 transformation)
    {
        Matrix4x4 viewMatrix = activeScreen.camera.viewMatrix;
        Matrix4x4 projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
            activeScreen.camera.fieldOfView * Mathf.Deg2Rad,
            Size.X / (float)Size.Y,
            activeScreen.camera.nearPlaneDistance,
            activeScreen.camera.farPlaneDistance
        );

        shader.Use(transformation, viewMatrix, projectionMatrix);
        GL.BindVertexArray(mesh.VertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
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
