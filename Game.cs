using System.Text;

namespace TextDungeon
{
    internal class Game
    {
        private const int RoomsPerFloor = 5;

        private Player player;
        private int floor = 1;
        private int roomOnFloor = 0;
        private int roomsVisited = 0;

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // hogy az ékezetes betűk jól jelenjenek meg
            Console.WriteLine(Texts.Title);
            Console.WriteLine();

            string name = Input.ReadName("Add meg a neved: ");
            player = new Player(name);
            Console.WriteLine($"Üdv a dungeonben, {player.Name}!");

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                PrintStatus();
                Console.WriteLine("[1] Tovább  [2] Statok  [3] Potion  [4] Kilépés");
                int choice = Input.ReadInt(1, 4);
                Console.WriteLine();
                switch (choice)
                {
                    case 1:
                        GoForward();
                        break;
                    case 2:
                        player.PrintStats();
                        break;
                    case 3:
                        DrinkPotion();
                        break;
                    case 4:
                        running = false;
                        break;
                }
            }

            GameOver();
        }

        private void PrintStatus()
        {
            Console.WriteLine($"{player.Name} | HP {player.Hp}/{player.MaxHp} | Floor {floor} | Szoba {roomOnFloor}/{RoomsPerFloor} | Gold {player.Gold} | Potion {player.Potions}");
        }

        private void GoForward()
        {
            roomOnFloor++;
            roomsVisited++;
            Console.WriteLine("Továbbmész egy újabb szobába.");
        }

        private void DrinkPotion()
        {
            if (player.DrinkPotion())
            {
                Console.WriteLine($"Megittál egy potion-t. HP: {player.Hp}/{player.MaxHp}");
            }
            else
            {
                Console.WriteLine("Nincs potion-öd!");
            }
        }

        private void GameOver()
        {
            Console.WriteLine();
            Console.WriteLine("=========== GAME OVER ===========");
            Console.WriteLine($"Elért floor:      {floor}");
            Console.WriteLine($"Bejárt szobák:    {roomsVisited}");
            Console.WriteLine($"Gold:             {player.Gold}");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez.");
            Input.WaitForKey();
        }
    }
}
