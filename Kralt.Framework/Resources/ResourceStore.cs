using Kralt.Framework.Graphics.Rendering;
using System.Collections.Generic;

#nullable disable

namespace Kralt.Framework.Resources
{
    public class ResourceStore
    {
        public ShaderProgram fallbackShader;
        public readonly Dictionary<string, ShaderProgram> shaders = [];
    }
}
