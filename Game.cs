using System.Text;

namespace TextDungeon
{
    internal class Game
    {
        private const int RoomsPerFloor = 5;
        private const int BossFloorInterval = 5;

        private readonly Random random = new Random();
        private readonly Shop shop = new Shop();
        private readonly Leaderboard leaderboard = new Leaderboard();
        private Player player;
        private DifficultySettings settings;
        private int floor = 1;
        private int roomOnFloor = 0;
        private int roomsVisited = 0;

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // hogy az ékezetes betűk jól jelenjenek meg
            Console.WriteLine(Texts.Title);
            Console.WriteLine();
            leaderboard.Load();

            string name = Input.ReadName("Add meg a neved: ");
            Console.WriteLine("Válassz difficulty-t:");
            Console.WriteLine("[1] Easy  [2] Normal  [3] Hard");
            Difficulty difficulty = (Difficulty)(Input.ReadInt(1, 3) - 1);
            settings = DifficultySettings.For(difficulty);

            player = CreatePlayer(name);
            Console.WriteLine($"Üdv a dungeonben, {player.Name} the {player.ClassName}! (Difficulty: {settings.Difficulty})");

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

        /// <summary>Az egyetlen hely, ahol a kaszt számít: létrejön a megfelelő osztály példánya.</summary>
        private Player CreatePlayer(string name)
        {
            Console.WriteLine("Válassz kasztot:");
            Console.WriteLine("[1] Warrior (26 HP, 3-5 sebzés, -1 kapott sebzés)");
            Console.WriteLine("[2] Mage    (16 HP, 2-4 sebzés, +1 HP minden szobában)");
            Console.WriteLine("[3] Rogue   (20 HP, 2-5 sebzés, 20% dodge)");
            switch (Input.ReadInt(1, 3))
            {
                case 1:
                    return new Warrior(name, settings);
                case 2:
                    return new Mage(name, settings);
                default:
                    return new Rogue(name, settings);
            }
        }

        private void PrintStatus()
        {
            Console.WriteLine($"{player.Name} | HP {player.Hp}/{player.MaxHp} | Floor {floor} | Szoba {roomOnFloor}/{RoomsPerFloor} | Gold {player.Gold} | Potion {player.Potions} | Lvl {player.Level} | XP {player.Xp}/{player.XpToNextLevel}");
        }

        private void GoForward()
        {
            List<Door> doors = new List<Door>();
            bool bossRoom = floor % BossFloorInterval == 0 && roomOnFloor == RoomsPerFloor - 1;
            if (bossRoom)
            {
                // Minden 5. floor utolsó szobája: egyetlen ajtó vezet a boss-hoz.
                doors.Add(new Door(RoomType.Boss, RoomTable.RandomHint(RoomType.Boss, random)));
                Console.WriteLine("Egyetlen ajtó áll előtted. Innen nincs visszaút...");
            }
            else
            {
                int doorCount = random.Next(2, 4);
                for (int i = 0; i < doorCount; i++)
                {
                    doors.Add(RoomTable.CreateDoor(random));
                }
                Console.WriteLine($"{doorCount} ajtó áll előtted:");
            }

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
            shop.Open(player);
        }

        private void Fight(Monster monster)
        {
            new Combat(player, monster, random, settings.FleeChance).Run();
        }

        private void EnterRoom(RoomType type)
        {
            roomOnFloor++;
            roomsVisited++;
            Console.WriteLine(RoomTable.RandomDescription(type, random));
            string passive = player.OnEnterRoom();
            if (passive != null)
            {
                Console.WriteLine(passive);
            }

            switch (type)
            {
                case RoomType.Monster:
                    Fight(MonsterFactory.Create(floor, random, settings));
                    break;
                case RoomType.Boss:
                    Fight(MonsterFactory.CreateBoss(floor, random, settings));
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

        /// <summary>Score = (10 × elért floor + összes összegyűjtött gold) × difficulty szorzó, lefelé kerekítve.</summary>
        private int CalculateScore()
        {
            int raw = 10 * floor + player.TotalGoldEarned;
            return (int)Math.Floor(raw * settings.ScoreMultiplier);
        }

        private void GameOver()
        {
            Console.WriteLine();
            Console.WriteLine("=========== GAME OVER ===========");
            Console.WriteLine($"Elért floor:      {floor}");
            Console.WriteLine($"Bejárt szobák:    {roomsVisited}");
            Console.WriteLine($"Gold:             {player.Gold}");
            int score = CalculateScore();
            Console.WriteLine($"Score:            {score}  ({settings.Difficulty}, x{settings.ScoreMultiplier})");
            Console.WriteLine("=================================");
            Console.WriteLine();

            if (player.IsAlive)
            {
                Console.WriteLine("Kiléptél, ezért az eredményed nem kerül a leaderboard-ra.");
            }
            else
            {
                leaderboard.Add(player.Name, score, settings.Difficulty);
            }
            leaderboard.Print();
            Console.WriteLine();
            Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez.");
            Input.WaitForKey();
        }
    }
}
