using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt
{
    public static class Primatives
    {
        public static Mesh Cube
        {
            get
            {
                Vertex[] unit_cube_v = [

                    // Front
                    new(-1f, 1f, 1f, Color.white, 0f, 1f), // top left
                    new(-1f, -1f, 1f, Color.white, 0f, 0f), // bottom left
                    new(1f, -1f, 1f, Color.white, 1f, 0f), // bottom right
                    new(1f, 1f, 1f, Color.white, 1f, 1f), // top right

                    // Back
                    new(1f, 1f, -1f, Color.white, 0f, 1f), // top left
                    new(1f, -1f, -1f, Color.white, 0f, 0f), // bottom left
                    new(-1f, -1f, -1f, Color.white, 1f, 0f), // bottom right
                    new(-1f, 1f, -1f, Color.white, 1f, 1f), // top right

                    // Top
                    new(-1f, 1f, -1f, Color.white, 0f, 1f), // top left
                    new(-1f, 1f, 1f, Color.white, 0f, 0f), // bottom left
                    new(1f, 1f, 1f, Color.white, 1f, 0f), // bottom right
                    new(1f, 1f, -1f, Color.white, 1f, 1f), // top right

                    // Bottom
                    new(-1f, -1f, 1f, Color.white, 0f, 1f), // top left
                    new(-1f, -1f, -1f, Color.white, 0f, 0f), // bottom left
                    new(1f, -1f, -1f, Color.white, 1f, 0f), // bottom right
                    new(1f, -1f, 1f, Color.white, 1f, 1f), // top right

                    // Left
                    new(-1f, 1f, -1f, Color.white, 0f, 1f), // top left
                    new(-1f, -1f, -1f, Color.white, 0f, 0f), // bottom left
                    new(-1f, -1f, 1f, Color.white, 1f, 0f), // bottom right
                    new(-1f, 1f, 1f, Color.white, 1f, 1f), // top right

                    // Right
                    new(1f, 1f, 1f, Color.white, 0f, 1f), // top left
                    new(1f, -1f, 1f, Color.white, 0f, 0f), // bottom left
                    new(1f, -1f, -1f, Color.white, 1f, 0f), // bottom right
                    new(1f, 1f, -1f, Color.white, 1f, 1f), // top right

                ];

                uint[] unit_cube_i = [

                    // Front
                    0, 1, 3, 1, 2, 3,

                    // Back
                    4, 5, 7, 5, 6, 7,

                    // Top
                    8, 9, 11, 9, 10, 11,

                    // Bottom
                    12, 13, 15, 13, 14, 15,

                    // Left
                    16, 17, 19, 17, 18, 19,

                    // Right
                    20, 21, 23, 21, 22, 23,

                ];

                return new Mesh(unit_cube_v, unit_cube_i);
            }
        }
    }
}
