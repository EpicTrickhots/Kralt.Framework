using System.Collections.Generic;
using Kralt.Framework.Graphics;
using Kralt.Framework.Physics;
using Kralt.Mathematics;

#nullable disable

namespace Kralt
{
    public class KraltObject : Transformable, IRenderable
    {
        private HashSet<Vector3Int> points = [];

        public HashSet<AABB> collisionAABB = [];

        private readonly Mesh mesh;
        private readonly Material material;

        private readonly List<Vertex> vertices = [];
        private readonly List<uint> indices = [];

        public KraltObject(HashSet<Vector3Int> points)
        {
            this.points = points;

            foreach (Vector3Int block in points)
            {
                int numFacesRendered = 0;

                foreach (FaceData face in faces)
                {
                    if (points.Contains(block + face.Direction)) // Block exists on this side, skip the face
                        continue;

                    uint start = (uint)vertices.Count;

                    for (int i = 0; i < 4; i++)
                    {
                        Vertex v = face.Vertices[i];
                        vertices.Add(new(block + v.position, Color.white, v.uv0));
                    }

                    indices.Add(start + 0);
                    indices.Add(start + 1);
                    indices.Add(start + 3);

                    indices.Add(start + 1);
                    indices.Add(start + 2);
                    indices.Add(start + 3);

                    numFacesRendered++;
                }

                // If zero of the faces for this voxel get rendered, we can skip collision generation
                if(numFacesRendered != 0)
                {
                    // Generate AABB collision for this voxel
                    Vector3 min = new(block.x - 0.5f, block.y - 0.5f, block.z - 0.5f);
                    Vector3 max = new(block.x + 0.5f, block.y + 0.5f, block.z + 0.5f);

                    AABB voxelAABB = new(min, max);
                    collisionAABB.Add(voxelAABB);
                }
            }

            mesh = new Mesh([.. vertices], [.. indices]);
            material = Resources.materials["kralt-grid"];
        }

        public void Render()
        {
            if (mesh == null)
                return;

            Renderer.Draw(mesh, material, worldMatrix);
        }

        private static readonly FaceData[] faces =
        [
            new(Vector3Int.right,
            [
                new Vertex(0.5f, -0.5f, -0.5f, 1f, 0f),
                new Vertex(0.5f, 0.5f, -0.5f, 1f, 1f),
                new Vertex(0.5f, 0.5f, 0.5f, 0f, 1f),
                new Vertex(0.5f, -0.5f, 0.5f, 0f, 0f)
            ]),
            new(-Vector3Int.right,
            [
                new Vertex(-0.5f, -0.5f, 0.5f, 1f, 0f),
                new Vertex(-0.5f, 0.5f, 0.5f, 1f, 1f),
                new Vertex(-0.5f, 0.5f, -0.5f, 0f, 1f),
                new Vertex(-0.5f, -0.5f, -0.5f, 0f, 0f)
            ]),
            new(Vector3Int.up,
            [
                new Vertex(-0.5f, 0.5f, 0.5f, 1f, 0f),
                new Vertex(0.5f, 0.5f, 0.5f, 1f, 1f),
                new Vertex(0.5f, 0.5f, -0.5f, 0f, 1f),
                new Vertex(-0.5f, 0.5f, -0.5f, 0f, 0f)
            ]),
            new(-Vector3Int.up,
            [
                new Vertex(-0.5f, -0.5f, -0.5f, 1f, 0f),
                new Vertex(0.5f, -0.5f, -0.5f, 1f, 1f),
                new Vertex(0.5f, -0.5f, 0.5f, 0f, 1f),
                new Vertex(-0.5f, -0.5f, 0.5f, 0f, 0f)
            ]),
            new(Vector3Int.forward,
            [
                new Vertex(0.5f, -0.5f, 0.5f, 1f, 0f),
                new Vertex(0.5f, 0.5f, 0.5f, 1f, 1f),
                new Vertex(-0.5f, 0.5f, 0.5f, 0f, 1f),
                new Vertex(-0.5f, -0.5f, 0.5f, 0f, 0f)
            ]),
            new(-Vector3Int.forward,
            [
                new Vertex(-0.5f, -0.5f, -0.5f, 1f, 0f),
                new Vertex(-0.5f, 0.5f, -0.5f, 1f, 1f),
                new Vertex(0.5f, 0.5f, -0.5f, 0f, 1f),
                new Vertex(0.5f, -0.5f, -0.5f, 0f, 0f)
            ])
        ];

        public readonly struct FaceData(Vector3Int direction, Vertex[] vertices)
        {
            public Vector3Int Direction { get; } = direction;
            public Vertex[] Vertices { get; } = vertices;
        }
    }
}
