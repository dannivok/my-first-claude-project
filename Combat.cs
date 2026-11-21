namespace TextDungeon
{
    internal enum CombatResult
    {
        Won,
        Fled,
        Died,
    }

    /// <summary>
    /// Egy harc a játékos és egy monster között.
    /// </summary>
    internal class Combat
    {
        private const int FleeChance = 50;

        private readonly Player player;
        private readonly Monster monster;
        private readonly Random random;

        public Combat(Player player, Monster monster, Random random)
        {
            this.player = player;
            this.monster = monster;
            this.random = random;
        }

        public CombatResult Run()
        {
            Console.WriteLine($"Egy {monster.Name} támad rád! (HP {monster.Hp}, sebzés {monster.MinDamage}-{monster.MaxDamage})");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"{player.Name}: HP {player.Hp}/{player.MaxHp} | {monster.Name}: HP {monster.Hp}/{monster.MaxHp} | Potion {player.Potions}");
                Console.WriteLine("[1] Attack  [2] Flee  [3] Potion");
                int choice = Input.ReadInt(1, 3);

                bool monsterStrikes;
                switch (choice)
                {
                    case 1:
                        int dealt = monster.TakeDamage(player.RollDamage(random));
                        Console.WriteLine($"Megütöd: {monster.Name} -{dealt} HP.");
                        monsterStrikes = monster.IsAlive;
                        break;
                    case 2:
                        if (random.Next(100) < FleeChance)
                        {
                            Console.WriteLine("Sikerült elmenekülnöd! Jutalom nélkül hagyod el a szobát.");
                            return CombatResult.Fled;
                        }
                        Console.WriteLine("Nem sikerült elmenekülni!");
                        monsterStrikes = true;
                        break;
                    default:
                        if (!player.DrinkPotion())
                        {
                            Console.WriteLine("Nincs potion-öd! (Nem telt el kör.)");
                            continue;
                        }
                        Console.WriteLine("Megittál egy potion-t.");
                        monsterStrikes = true;
                        break;
                }

                if (monsterStrikes)
                {
                    Console.WriteLine(monster.Attack(player, random));
                }

                Console.WriteLine($"-> {player.Name}: {player.Hp} HP, {monster.Name}: {monster.Hp} HP");

                if (!monster.IsAlive)
                {
                    int gold = random.Next(3, 9);
                    player.AddGold(gold);
                    Console.WriteLine($"Legyőzted: {monster.Name}! Jutalom: {gold} gold és {monster.XpReward} XP.");
                    int levelUps = player.AddXp(monster.XpReward);
                    if (levelUps > 0)
                    {
                        Console.WriteLine($"LEVEL UP! Level {player.Level}: max HP {player.MaxHp}, sebzés {player.MinDamage}-{player.MaxDamage}, HP feltöltve.");
                    }
                    return CombatResult.Won;
                }
                if (!player.IsAlive)
                {
                    Console.WriteLine($"{monster.Name} legyőzött...");
                    return CombatResult.Died;
                }
            }
        }
    }
}
