using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    // TODO:
    //  - Remove matrix transformations from shader.Use()
    //  - Add support for multiple texture target types (Texture3D, Texture2DArray, Cubemap)
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
                if (location == -1) continue;

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

            int unit = 0;
            foreach(var (name, texture) in textures)
            {
                int location = shader[name];
                if (location == -1) continue;

                texture.Use(TextureUnit.Texture0 + unit);
                GL.Uniform1(location, unit);
                unit++;
            }
        }

        #region Material property setters

        public void SetFloat(string name, float value) => properties[name] = value;

        public void SetInt(string name, int value) => properties[name] = value;

        public void SetVector(string name, Mathematics.Vector3 value) => properties[name] = value;

        public void SetColor(string name, Color value) => properties[name] = value;

        public void SetMatrix(string name, Matrix4x4 value) => properties[name] = value;

        public void SetTexture(string name, Texture value) => properties[name] = value;

        #endregion
    }
}
