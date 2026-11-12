using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace Kralt.Framework.Graphics.Rendering
{
    public class ShaderProgram : IDisposable
    {
        internal int Vertex { get; }
        internal int Fragment { get; }
        internal int Handle { get; }

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
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public static ShaderProgram Compile(string dir, string name)
        {
            string vertexPath = Path.Combine(dir, $"{name}.vert");
            string fragmentPath = Path.Combine(dir, $"{name}.frag");

            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            return new ShaderProgram(vertexCode, fragmentCode);
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
