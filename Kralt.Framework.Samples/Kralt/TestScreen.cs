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

            Mesh cube = Primatives.Cube;

            Add(new MeshRenderer
            {
                mesh = cube,
                position = Vector3.zero,
                rotation = Quaternion.Euler(22f, 110f, 45f)
            });

            Add(new MeshRenderer
            {
                mesh = cube,
                position = new(0f, 0f, -5f),
                rotation = Quaternion.Euler(0f, 0f, 0f),
                material = Resources.materials["kralt-grid"]
            });

            Add(new MeshRenderer
            {
                mesh = cube,
                position = new(-6f, -2f, 8f),
                rotation = Quaternion.Euler(5f, 70f, 0f)
            });
        }

        public override void Update()
        {
            base.Update();

            camera?.HandleCameraMovement();
        }
    }
}
