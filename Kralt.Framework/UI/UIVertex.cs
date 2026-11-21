using System.Runtime.InteropServices;
using Kralt.Mathematics;

namespace Kralt.Framework.UI
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UIVertex
    {
        public Vector2 position { get; }
        public Color color { get; }
        public Vector2 uv0 { get; }
    }
}
