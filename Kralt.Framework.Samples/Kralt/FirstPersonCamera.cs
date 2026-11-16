using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;
using Kralt.Mathematics;

namespace Kralt
{
    internal class FirstPersonCamera : Camera
    {
        float yaw = 0f;
        float pitch = 0f;

        internal void HandleCameraMovement()
        {
            Vector2 delta = Input.WasdDelta;

            float up = Input.GetKey(Key.Space) ? 1f : 0f;
            float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

            Vector3 movement = right * delta.x + forward * delta.y + Vector3.up * (up - down);

            localPosition += movement * Time.deltaTime * 10f;

            // Mouse look
            Vector2 mouseDelta = Input.MouseDelta;
            yaw -= mouseDelta.x * 0.002f;
            pitch -= mouseDelta.y * 0.002f;

            float maxPitch = 89.0f * Mathf.Deg2Rad;
            pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

            Quaternion rotationY = Quaternion.FromAxisAngle(Vector3.up, yaw);
            Quaternion rotationX = Quaternion.FromAxisAngle(Vector3.right, pitch);

            localRotation = rotationY * rotationX;
        }
    }
}
