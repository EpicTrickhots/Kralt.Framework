using Kralt.Framework;
using Kralt.Framework.InputHandling;

namespace Kralt
{
    internal class KraltGame : Application
    {
        internal KraltGame() : base("Kralt", 1200, 800)
        {
            scenes = [ new TestScreen() ];
        }

        protected override void Start()
        {
            Mouse.lockState = CursorLockMode.Locked;
        }
    }
}
