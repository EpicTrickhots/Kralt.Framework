using Kralt.Framework;
using Kralt.Framework.UI;
using Kralt.Mathematics;

namespace UserInterfaceTest;

internal class GuiApplication : Application
{
    public GuiApplication() : base("UI Test", 1200, 800)
    {
        activeUI = [
            new Drawable()
            {
                position = new(200f, 0f),
                size = new(100f, 100f),
                color = new(0f, 1f, 0f)
            },
            new Drawable()
            {
                position = new(-200f, 0f),
                size = new(100f, 100f),
                color = new(1f, 0f, 0f)
            },
            new Drawable()
            {
                position = new(0f, 200f),
                size = new(100f, 100f),
                color = new(0f, 0f, 1f)
            },
            new Drawable()
            {
                position = new(0f, -200f),
                size = new(100f, 100f),
                color = new(1f, 1f, 1f)
            },
        ];
    }
}
