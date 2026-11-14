using Kralt.Framework.Graphics;
using Kralt.Framework.Screens;
using Kralt.Mathematics;

namespace Kralt
{
    internal class TestScreen : Screen
    {
        readonly Color color_front = new Color(0.203f, 0.431f, 0.509f);
        readonly Color color_back = new Color(0.969f, 0.808f, 0.447f);

        readonly Color color_top = new Color(1.000f, 0.960f, 0.819f);
        readonly Color color_bottom = new Color(0.811f, 0.309f, 0.266f);

        readonly Color color_right = new Color(0.968f, 0.447f, 0.411f);
        readonly Color color_left = new Color(0.231f, 0.254f, 0.388f);

        public override void Initialize()
        {
            camera.position = Vector3.forward * -5f;

            Vertex[] unit_cube_v = [

                // Front
                new(-1f, 1f, 1f, color_front, 0f, 1f), // top left
                new(-1f, -1f, 1f, color_front, 0f, 0f), // bottom left
                new(1f, -1f, 1f, color_front, 1f, 0f), // bottom right
                new(1f, 1f, 1f, color_front, 1f, 1f), // top right

                // Back
                new(1f, 1f, -1f, color_back, 0f, 1f), // top left
                new(1f, -1f, -1f, color_back, 0f, 0f), // bottom left
                new(-1f, -1f, -1f, color_back, 1f, 0f), // bottom right
                new(-1f, 1f, -1f, color_back, 1f, 1f), // top right

                // Top
                new(-1f, 1f, -1f, color_top, 0f, 1f), // top left
                new(-1f, 1f, 1f, color_top, 0f, 0f), // bottom left
                new(1f, 1f, 1f, color_top, 1f, 0f), // bottom right
                new(1f, 1f, -1f, color_top, 1f, 1f), // top right

                // Bottom
                new(-1f, -1f, 1f, color_bottom, 0f, 1f), // top left
                new(-1f, -1f, -1f, color_bottom, 0f, 0f), // bottom left
                new(1f, -1f, -1f, color_bottom, 1f, 0f), // bottom right
                new(1f, -1f, 1f, color_bottom, 1f, 1f), // top right

                // Left
                new(-1f, 1f, -1f, color_left, 0f, 1f), // top left
                new(-1f, -1f, -1f, color_left, 0f, 0f), // bottom left
                new(-1f, -1f, 1f, color_left, 1f, 0f), // bottom right
                new(-1f, 1f, 1f, color_left, 1f, 1f), // top right

                // Right
                new(1f, 1f, 1f, color_right, 0f, 1f), // top left
                new(1f, -1f, 1f, color_right, 0f, 0f), // bottom left
                new(1f, -1f, -1f, color_right, 1f, 0f), // bottom right
                new(1f, 1f, -1f, color_right, 1f, 1f), // top right

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

            GameObject g1 = new MeshRenderer
            {
                mesh = new Mesh(unit_cube_v, unit_cube_i),
                position = Vector3.zero,
                rotation = Quaternion.Euler(22f, 110f, 45f)
            };

            Append(g1);

            GameObject g2 = new MeshRenderer
            {
                mesh = new Mesh(unit_cube_v, unit_cube_i),
                position = new(9f, 0f, 2f),
                rotation = Quaternion.Euler(0f, 0f, 0f)
            };

            Append(g2);
        }
    }
}
