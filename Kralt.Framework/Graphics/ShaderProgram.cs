using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    // TODO: Add shader preprocessor support for #include statements
    public class ShaderProgram : IDisposable
    {
        internal int Vertex { get; }

        internal int Fragment { get; }

        internal int Handle { get; }

        internal Dictionary<string, int> uniformLocations;

        public int this[string name] => uniformLocations.TryGetValue(name, out int v) ? v : -1;

        private bool disposedValue = false;

        internal ShaderProgram(string vsource, string fsource)
        {
            Vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(Vertex, vsource);

            Fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(Fragment, fsource);

            GL.CompileShader(Vertex);

            GL.GetShader(Vertex, ShaderParameter.CompileStatus, out int vsuccess);
            if (vsuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(Vertex);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(Fragment);

            GL.GetShader(Fragment, ShaderParameter.CompileStatus, out int fsuccess);
            if (fsuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(Fragment);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, Vertex);
            GL.AttachShader(Handle, Fragment);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int hsuccess);
            if(hsuccess == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, Vertex);
            GL.DetachShader(Handle, Fragment);
            GL.DeleteShader(Vertex);
            GL.DeleteShader(Fragment);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numUniforms);

            uniformLocations = [];

            for(int i = 0; i < numUniforms; i++)
            {
                string name = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, name);
                uniformLocations.Add(name, location);
            }
        }

        internal virtual void Use()
        {
            GL.UseProgram(Handle);
        }

        public static ShaderProgram Load(string dir, string name)
        {
            string vertexPath = Path.Combine(dir, $"{name}.vert");
            string fragmentPath = Path.Combine(dir, $"{name}.frag");

            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            return new ShaderProgram(vertexCode, fragmentCode);
        }

        internal void SetMatrix4x4(string id, Matrix4x4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[id], true, ref Unsafe.As<Matrix4x4, OpenTK.Mathematics.Matrix4>(ref data));
        }

        internal void SetVec2(string id, Vector2 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[id], data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        ~ShaderProgram()
        {
            if (!disposedValue)
            {
                Console.WriteLine("GPU Resource Leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
