using Kralt.Framework;

namespace UserInterfaceTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Application game = new GuiApplication())
            {
                game.Run();
            }
        }
    }
}
