using System.Text;

namespace TextDungeon
{
    internal class Game
    {
        private const int RoomsPerFloor = 5;

        private readonly Random random = new Random();
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
                        running = player.IsAlive;
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
            int doorCount = random.Next(2, 4);
            List<Door> doors = new List<Door>();
            for (int i = 0; i < doorCount; i++)
            {
                doors.Add(RoomTable.CreateDoor(random));
            }

            Console.WriteLine($"{doorCount} ajtó áll előtted:");
            for (int i = 0; i < doors.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {doors[i].Hint}");
            }
            int choice = Input.ReadInt(1, doors.Count);
            Console.WriteLine();
            EnterRoom(doors[choice - 1].RoomType);

            if (player.IsAlive && roomOnFloor >= RoomsPerFloor)
            {
                NextFloor();
            }
        }

        private void NextFloor()
        {
            floor++;
            roomOnFloor = 0;
            Console.WriteLine();
            Console.WriteLine($"*** Lejutottál a(z) {floor}. floor-ra! A monsterek itt erősebbek. ***");
        }

        private void EnterRoom(RoomType type)
        {
            roomOnFloor++;
            roomsVisited++;
            Console.WriteLine(RoomTable.RandomDescription(type, random));

            switch (type)
            {
                case RoomType.Monster:
                    Monster monster = MonsterFactory.Create(floor, random);
                    new Combat(player, monster, random).Run();
                    break;
                case RoomType.Gold:
                    int gold = random.Next(5, 16);
                    player.AddGold(gold);
                    Console.WriteLine($"Találtál {gold} gold-ot!");
                    break;
                case RoomType.Empty:
                    Console.WriteLine("Itt nincs semmi érdekes.");
                    break;
                case RoomType.Potion:
                    player.AddPotion();
                    Console.WriteLine("Találtál egy potion-t!");
                    break;
                case RoomType.Trap:
                    int lost = player.TakeDamage(random.Next(2, 6));
                    Console.WriteLine($"Csapda! -{lost} HP. HP: {player.Hp}/{player.MaxHp}");
                    if (!player.IsAlive)
                    {
                        Console.WriteLine("A csapda végzett veled...");
                    }
                    break;
                case RoomType.Fountain:
                    int healed = player.Heal(10);
                    Console.WriteLine($"A forrás vize gyógyít: +{healed} HP. HP: {player.Hp}/{player.MaxHp}");
                    break;
            }
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
