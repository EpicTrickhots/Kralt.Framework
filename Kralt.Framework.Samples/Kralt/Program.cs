using Kralt.Framework;

namespace Kralt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Application game = new KraltGame())
            {
                game.Run(0);
            }
        }
    }
}
