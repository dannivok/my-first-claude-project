using System.Text;

namespace TextDungeon
{
    internal class Game
    {
        private Player player;

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // hogy az ékezetes betűk jól jelenjenek meg
            Console.WriteLine(Texts.Title);
            Console.WriteLine();

            string name = Input.ReadName("Add meg a neved: ");
            player = new Player(name);
            Console.WriteLine($"Üdv a dungeonben, {player.Name}!");
            Console.WriteLine();
            player.PrintStats();

            Console.WriteLine();
            Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez.");
            Input.WaitForKey();
        }
    }
}
