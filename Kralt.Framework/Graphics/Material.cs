using System.Collections.Generic;
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

        public void Use()
        {
            // shader.Use();

            foreach(var (name, value) in properties)
            {
                
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
