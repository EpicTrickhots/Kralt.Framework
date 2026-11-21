namespace Kralt.Framework.Physics;

[System.Flags]
public enum CollisionFlags
{
    None = 0,
    Sides = 1 << 0,
    Above = 1 << 1,
    Below = 1 << 2
}
