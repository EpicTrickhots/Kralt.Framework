using System.Collections.Generic;
using System.Threading;
using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt
{
    internal class TestScene : Scene
    {
        private KraltPlayerController? player;

        public override void Initialize()
        {
            player = new KraltPlayerController()
            {
                position = Vector3.up,
                localRotation = Quaternion.Euler(0f, -155.623f, 0f)
            };

            outputCamera = player[0] as Camera;

            Transformable kraltObjectContainer = new() {
                new KraltObject(new()
                {
                    new(0, 0, 0), new(1, 0, 0), new(2, 0, 0), new(3, 0, 0), new(4, 0, 0), new(5, 0, 0),
                    new(0, 0, 1), new(1, 0, 1), new(2, 0, 1), new(3, 0, 1), new(4, 0, 1), new(5, 0, 1),
                    new(0, 0, 2), new(1, 0, 2), new(2, 0, 2), new(3, 0, 2), new(4, 0, 2), new(5, 0, 2),
                    new(0, 0, 3), new(1, 0, 3), new(2, 0, 3), new(3, 0, 3), new(4, 0, 3), new(5, 0, 3),
                    new(0, 0, 4), new(1, 0, 4), new(2, 0, 4), new(3, 0, 4), new(4, 0, 4), new(5, 0, 4),
                    new(0, 0, 5), new(1, 0, 5), new(2, 0, 5), new(3, 0, 5), new(4, 0, 5), new(5, 0, 5),
                    new(0, 0, 6), new(1, 0, 6), new(2, 0, 6), new(3, 0, 6), new(4, 0, 6), new(5, 0, 6),

                    new(0, 1, 6), new(1, 1, 6), new(2, 1, 6), new(3, 1, 6), new(4, 1, 6), new(5, 1, 6),
                    new(0, 2, 6), new(1, 2, 6), new(2, 2, 6), new(3, 2, 6), new(4, 2, 6), new(5, 2, 6),
                    new(0, 3, 6), new(1, 3, 6), new(2, 3, 6), new(3, 3, 6), new(4, 3, 6), new(5, 3, 6),
                    new(0, 4, 6), new(1, 4, 6), new(2, 4, 6), new(3, 4, 6), new(4, 4, 6), new(5, 4, 6),
                })
            };

            player.collisionObjects = kraltObjectContainer;
            Add(player);
            Add(kraltObjectContainer);
        }
    }
}
