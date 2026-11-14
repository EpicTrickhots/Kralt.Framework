using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Kralt.Mathematics;
using System.Collections.Generic;

namespace Kralt.Framework.Graphics.Rendering
{
    /// <summary>
    /// Internal container for handles that point at GPU bound OpenGL shader programs
    /// </summary>
    public class ShaderProgram : IDisposable
    {
        /// <summary>
        /// GPU handle for this program's vertex shader
        /// </summary>
        internal int Vertex { get; }

        /// <summary>
        /// GPU handle for this proram's fragment shader
        /// </summary>
        internal int Fragment { get; }

        /// <summary>
        /// GPU handle for this shader program
        /// </summary>
        internal int Handle { get; }

        internal Dictionary<string, int> uniformLocations;

        private bool disposedV = false;

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

        internal virtual void Use(Matrix4x4 transformation, Matrix4x4 view, Matrix4x4 proj)
        {
            GL.UseProgram(Handle);

            SetMatrix4x4("TR_MESH", transformation);
            SetMatrix4x4("TR_VIEW", view);
            SetMatrix4x4("TR_PROJ", proj);
        }

        internal static ShaderProgram Compile(string dir, string name)
        {
            string vertexPath = Path.Combine(dir, $"{name}.vert");
            string fragmentPath = Path.Combine(dir, $"{name}.frag");

            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            return new ShaderProgram(vertexCode, fragmentCode);
        }

        internal void SetInt(string id, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[id], data);
        }

        internal void SetFloat(string id, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[id], data);
        }

        internal void SetMatrix4x4(string id, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[id], true, ref data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedV)
            {
                GL.DeleteProgram(Handle);
                disposedV = true;
            }
        }

        ~ShaderProgram()
        {
            if (!disposedV)
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
