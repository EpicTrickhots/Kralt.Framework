using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    public class Transformable
    {
        public Vector3 position;

        public Quaternion rotation;

        public Vector3 scale;

        public Vector3 up => Vector3.Normalize(Vector3.Transform(Vector3.UnitY, rotation));

        public Vector3 down => Vector3.Normalize(Vector3.Transform(-Vector3.UnitY, rotation));

        public Vector3 forward => Vector3.Normalize(Vector3.Transform(-Vector3.UnitZ, rotation));

        public Vector3 backward => Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, rotation));

        public Vector3 right => Vector3.Normalize(Vector3.Transform(Vector3.UnitX, rotation));

        public Vector3 left => Vector3.Normalize(Vector3.Transform(-Vector3.UnitX, rotation));

        public Matrix4x4 worldMatrix
        {
            get
            {
                Matrix4x4 s = Matrix4x4.CreateScale(scale);
                Matrix4x4 r = Matrix4x4.CreateFromQuaternion(rotation);
                Matrix4x4 t = Matrix4x4.CreateTranslation(position);
                return s * r * t;
            }
        }

        public Transformable()
        {
            position = Vector3.zero;
            rotation = Quaternion.Identity;
            scale = Vector3.one;
        }
    }
}
