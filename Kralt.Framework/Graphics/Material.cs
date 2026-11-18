using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    // Provide 2 dictionaries for shader properties
    // Add functionality for multiple materials using the same shader
    // TODO: Remove matrix transformations from shader.Use()
    public class Material(ShaderProgram shader)
    {
        public ShaderProgram shader { get; } = shader;

        private readonly Dictionary<string, object> properties = [];

        private readonly Dictionary<string, Texture> textures = [];

        public void Apply()
        {
            shader.Use();

            foreach(var (name, value) in properties)
            {
                int location = shader[name];

                if (location == -1)
                    continue;

                switch (value)
                {
                    case float f:
                    GL.Uniform1(location, f);
                    break;

                    case int f:
                    GL.Uniform1(location, f);
                    break;

                    case Mathematics.Vector3 f:
                    GL.Uniform3(location, f);
                    break;

                    case Color f:
                    GL.Uniform4(location, f);
                    break;

                    case Matrix4 f:
                    GL.UniformMatrix4(location, false, ref f);
                    break;
                }
            }
        }

        #region Material property setters

        public void SetFloat(string name, float value) => properties[name] = value;

        public void SetInt(string name, int value) => properties[name] = value;

        public void SetVector(string name, Vector3 value) => properties[name] = value;

        public void SetColor(string name, Color value) => properties[name] = value;

        public void SetMatrix(string name, Matrix4x4 value) => properties[name] = value;

        public void SetTexture(string name, Texture value) => properties[name] = value;

        #endregion
    }
}
