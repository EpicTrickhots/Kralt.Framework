using Kralt.Framework.Graphics;
using Kralt.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Kralt
{
    public class KraltObject : Transformable, IRenderable
    {
        private HashSet<Vector3Int> points = [];

        private Mesh mesh;
        public Material material;

        private readonly List<Vertex> vertices = [];
        private readonly List<uint> indices = [];

        public KraltObject(HashSet<Vector3Int> points)
        {
            this.points = points;

            foreach (Vector3Int block in points)
            {
                foreach (FaceData face in faces)
                {
                    if (points.Contains(block + face.Direction)) // Block exists on this side, skip the face
                        continue;

                    uint start = (uint)vertices.Count;

                    for (int i = 0; i < 4; i++)
                    {
                        Vertex v = face.Vertices[i];
                        vertices.Add(new(block + v.position, v.color, v.uv0));
                    }

                    indices.Add(start + 0);
                    indices.Add(start + 1);
                    indices.Add(start + 2);

                    indices.Add(start + 0);
                    indices.Add(start + 2);
                    indices.Add(start + 3);
                }
            }

            mesh = new Mesh([.. vertices], [.. indices]);
            material = Resources.materials["kralt-grid"];
        }

        public void Render(Renderer renderer)
        {
            if (mesh == null)
                return;

            renderer.Draw(mesh, material, worldMatrix);
        }

        private static readonly FaceData[] faces =
        [
            new(Vector3Int.right,
            [
                new Vertex(0.5f, -0.5f, -0.5f, Color.white, 0f, 0f),
                new Vertex(0.5f, 0.5f, -0.5f, Color.white, 0f, 1f),
                new Vertex(0.5f, 0.5f, 0.5f, Color.white, 1f, 1f),
                new Vertex(0.5f, -0.5f, 0.5f, Color.white, 1f, 0f)
            ]),
            new(-Vector3Int.right,
            [
                new Vertex(-0.5f, -0.5f, 0.5f, Color.white, 0f, 0f),
                new Vertex(-0.5f, 0.5f, 0.5f, Color.white, 0f, 1f),
                new Vertex(-0.5f, 0.5f, -0.5f, Color.white, 1f, 1f),
                new Vertex(-0.5f, -0.5f, -0.5f, Color.white, 1f, 0f)
            ]),
            new(Vector3Int.up,
            [
                new Vertex(-0.5f, 0.5f, 0.5f, Color.white, 0f, 0f),
                new Vertex(0.5f, 0.5f, 0.5f, Color.white, 0f, 1f),
                new Vertex(0.5f, 0.5f, -0.5f, Color.white, 1f, 1f),
                new Vertex(-0.5f, 0.5f, -0.5f, Color.white, 1f, 0f)
            ]),
            new(-Vector3Int.up,
            [
                new Vertex(-0.5f, -0.5f, -0.5f, Color.white, 0f, 0f),
                new Vertex(0.5f, -0.5f, -0.5f, Color.white, 0f, 1f),
                new Vertex(0.5f, -0.5f, 0.5f, Color.white, 1f, 1f),
                new Vertex(-0.5f, -0.5f, 0.5f, Color.white, 1f, 0f)
            ]),
            new(Vector3Int.forward,
            [
                new Vertex(0.5f, -0.5f, 0.5f, Color.white, 0f, 0f),
                new Vertex(0.5f, 0.5f, 0.5f, Color.white, 0f, 1f),
                new Vertex(-0.5f, 0.5f, 0.5f, Color.white, 1f, 1f),
                new Vertex(-0.5f, -0.5f, 0.5f, Color.white, 1f, 0f)
            ]),
            new(-Vector3Int.forward,
            [
                new Vertex(-0.5f, -0.5f, -0.5f, Color.white, 0f, 0f),
                new Vertex(-0.5f, 0.5f, -0.5f, Color.white, 0f, 1f),
                new Vertex(0.5f, 0.5f, -0.5f, Color.white, 1f, 1f),
                new Vertex(0.5f, -0.5f, -0.5f, Color.white, 1f, 0f)
            ])
        ];

        public readonly struct FaceData(Vector3Int direction, Vertex[] vertices)
        {
            public Vector3Int Direction { get; } = direction;
            public Vertex[] Vertices { get; } = vertices;
        }
    }
}
