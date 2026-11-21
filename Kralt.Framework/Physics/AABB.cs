using Kralt.Mathematics;

namespace Kralt.Framework.Physics;

public struct AABB(Vector3 min, Vector3 max)
{
    public Vector3 min = min;
    public Vector3 max = max;

    public readonly bool Intersects(AABB other) =>
            min.x < other.max.x && max.x > other.min.x &&
            min.y < other.max.y && max.y > other.min.y &&
            min.z < other.max.z && max.z > other.min.z;
}
