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

        }

        protected override void Update()
        {
            Vector2 delta = Input.WasdDelta;

            float up = Input.GetKey(Key.Space) ? 1f : 0f;
            float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

            Vector3 movement = activeScreen.camera.right * delta.x + activeScreen.camera.forward * delta.y + activeScreen.camera.up * (up - down);

            activeScreen.camera.position += movement * Time.deltaTime * 10f;
        }
    }
}
