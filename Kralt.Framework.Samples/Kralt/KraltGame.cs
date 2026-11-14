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
            Vector2 delta = Input.WasdDelta;

            float up = Input.GetKey(Key.Space) ? 1f : 0f;
            float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

            Vector3 movement = activeScreen.camera.right * delta.x + activeScreen.camera.forward * delta.y + activeScreen.camera.up * (up - down);

            activeScreen.camera.position += movement * Time.deltaTime * 10f;
        }

        Quaternion ClampRotationAroundAxis(Quaternion q)
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
