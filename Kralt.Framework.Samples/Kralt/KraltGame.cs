using Kralt.Framework;
using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using System;
using System.IO;

namespace Kralt
{
    internal class KraltGame : Application
    {
        public string resourcePath = Path.Combine(AppContext.BaseDirectory, "datafiles");

        internal KraltGame() : base("Kralt", 1200, 800)
        {
            scenes = [ new TestScreen() ];
        }

        protected override void GenerateApplicationResources()
        {
            Resources.textures.Add("kralt-grid", Texture.Load(Path.Combine(resourcePath, @"textures\kralt-grid.png")));
        }

        protected override void Start()
        {
            Mouse.lockState = CursorLockMode.Locked;
        }
    }
}
