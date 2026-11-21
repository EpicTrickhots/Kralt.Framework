using System.Collections.Generic;
using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt
{
    internal class TestScene : Scene
    {
        /*
        readonly Color color_front = new Color(0.203f, 0.431f, 0.509f);
        readonly Color color_back = new Color(0.969f, 0.808f, 0.447f);

        readonly Color color_top = new Color(1.000f, 0.960f, 0.819f);
        readonly Color color_bottom = new Color(0.811f, 0.309f, 0.266f);

        readonly Color color_right = new Color(0.968f, 0.447f, 0.411f);
        readonly Color color_left = new Color(0.231f, 0.254f, 0.388f);
        */

        private KraltPlayerController? player;

        public override void Initialize()
        {
            player = new KraltPlayerController()
            {
                position = Vector3.up * 2f
            };

            outputCamera = player[0] as Camera;

            Transformable kraltObjectContainer = new() {
                new KraltObject(new()
                {
                    new(0, 0, 0),
                    new(1, 0, 0),
                    new(1, 0, 1),
                    new(0, 0, 1),
                    new(-1, 0, 1),
                    new(-1, 0, 0),
                    new(-1, 0, -1),
                    new(0, 0, -1),
                    new(1, 0, -1),
                })
            };

            player.collisionObjects = kraltObjectContainer;
            Add(player);
            Add(kraltObjectContainer);
        }
    }
}
