using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

#nullable disable

namespace Kralt.Framework.Graphics
{
    public static class Resources
    {
        public static readonly Dictionary<string, ShaderProgram> shaders = [];

        public static readonly Dictionary<string, Texture> textures = [];
    }
}
