using Kralt.Framework;
using Kralt.Mathematics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Runtime;

namespace Kralt
{
    internal class KraltGame : Application
    {
        internal KraltGame() : base("Kralt", 1200, 800)
        {
            activeScreen = new TestScreen();
        }

        protected override void Start()
        {
            Mouse.lockState = CursorLockMode.Locked;
        }

        protected override void Update()
        {
            HandleCameraMovement();
        }

        float yaw = 0f;
        float pitch = 0f;

        private void HandleCameraMovement()
        {
            Vector2 delta = Input.WasdDelta;

            float up = Input.GetKey(Key.Space) ? 1f : 0f;
            float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

            Vector3 movement = activeScreen.camera.right * delta.x + activeScreen.camera.forward * delta.y + Vector3.up * (up - down);

            activeScreen.camera.position += movement * Time.deltaTime * 10f;

            // Mouse look
            Vector2 mouseDelta = Input.MouseDelta;
            yaw -= mouseDelta.x * 0.002f;
            pitch -= mouseDelta.y * 0.002f;

            float maxPitch = 89.0f * Mathf.Deg2Rad;
            pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

            Quaternion rotationY = Quaternion.FromAxisAngle(Vector3.up, yaw);
            Quaternion rotationX = Quaternion.FromAxisAngle(Vector3.right, pitch);

            activeScreen.camera.rotation = rotationY * rotationX;
        }
    }
}
