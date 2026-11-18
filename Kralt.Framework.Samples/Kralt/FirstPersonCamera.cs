using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;
using Kralt.Mathematics;
using System;

namespace Kralt
{
    internal class FirstPersonCamera : Transformable
    {
        private const float MOUSE_SENS_X = .002f;

        private const float MOUSE_SENS_Y = .002f;

        private const float MOVE_SPEED = 10f;

        Quaternion playerTargetRotation = Quaternion.identity;
        Quaternion cameraTargetRotation = Quaternion.identity;

        Camera camera;

        public FirstPersonCamera()
        {
            camera = new Camera();
            Add(camera);
        }

        internal void HandleCameraMovement()
        {
            Vector2 moveDelta = Input.WasdDelta;

            float up = Input.GetKey(Key.Space) ? 1f : 0f;
            float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

            Vector3 movement = right * moveDelta.x + forward * moveDelta.y + Vector3.up * (up - down);

            localPosition += movement * Time.deltaTime * MOVE_SPEED;

            // Mouse look
            Vector2 mouseDelta = Input.MouseDelta;

            float x = mouseDelta.y * MOUSE_SENS_X;
            float y = mouseDelta.x * MOUSE_SENS_Y;

            playerTargetRotation *= Quaternion.Euler(0.0f, -y, 0.0f);
            cameraTargetRotation *= Quaternion.Euler(-x, 0.0f, 0.0f);

            cameraTargetRotation = ClampRotationAroundAxis(cameraTargetRotation);

            localRotation = playerTargetRotation;
            camera.localRotation = cameraTargetRotation;
        }

        private static Quaternion ClampRotationAroundAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
