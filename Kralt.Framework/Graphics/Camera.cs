using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    public class Camera : Transformable
    {
        public float fieldOfView = 80f;
        public float nearPlaneDistance = 0.1f;
        public float farPlaneDistance = 100f;

        public Matrix4x4 viewMatrix => localMatrix.Inverted(); // Matrix4x4.LookAt(position, position + forward, up);
    }
}
