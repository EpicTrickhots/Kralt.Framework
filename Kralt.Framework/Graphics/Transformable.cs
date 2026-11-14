using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    public class Transformable
    {
        public Vector3 position;

        public Quaternion rotation;

        public Vector3 scale;

        public Vector3 up => (rotation * Vector3.up).Normalized();

        public Vector3 forward => (rotation * -Vector3.forward).Normalized();

        public Vector3 right => (rotation * Vector3.right).Normalized();

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
