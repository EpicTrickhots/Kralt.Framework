using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt
{
    internal class TestScreen : Scene
    {
        /*
        readonly Color color_front = new Color(0.203f, 0.431f, 0.509f);
        readonly Color color_back = new Color(0.969f, 0.808f, 0.447f);

        readonly Color color_top = new Color(1.000f, 0.960f, 0.819f);
        readonly Color color_bottom = new Color(0.811f, 0.309f, 0.266f);

        readonly Color color_right = new Color(0.968f, 0.447f, 0.411f);
        readonly Color color_left = new Color(0.231f, 0.254f, 0.388f);
        */

        private FirstPersonCamera? camera;

        public override void Initialize()
        {
            camera = new FirstPersonCamera();
            Add(camera);

            outputCamera = camera[0] as Camera;
            // outputCamera = camera;

            /*
            Add(new MeshRenderer
            {
                mesh = Primatives.Cube,
                position = Vector3.zero,
                rotation = Quaternion.Euler(22f, 110f, 45f)
            });
            */

            Add(new KraltObject([
                new(0, 0, 0),
                new(1, 0, 0),
                new(1, 0, 1),
                new(0, 0, 1),
                new(-1, 0, 1),
                new(-1, 0, 0),
                new(-1, 0, -1),
                new(0, 0, -1),
                new(1, 0, -1),
            ]));
        }

        public override void Update()
        {
            base.Update();

            camera?.HandleCameraMovement();
        }
    }
}
