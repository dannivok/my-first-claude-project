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
        private readonly Player player;
        private readonly Monster monster;
        private readonly Random random;
        private readonly int fleeChance;

        public Combat(Player player, Monster monster, Random random, int fleeChance)
        {
            this.player = player;
            this.monster = monster;
            this.random = random;
            this.fleeChance = fleeChance;
        }

        public CombatResult Run()
        {
            Console.WriteLine($"Egy {monster.Name} támad rád! (HP {monster.Hp}, sebzés {monster.MinDamage}-{monster.MaxDamage})");
            Console.WriteLine($"Különlegesség: {monster.AbilityDescription}");
            ISkill skill = player.Skill;
            skill.Reset();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"{player.Name}: HP {player.Hp}/{player.MaxHp} | {monster.Name}: HP {monster.Hp}/{monster.MaxHp} | Potion {player.Potions}");
                string skillState = skill.IsReady ? "kész" : $"cooldown: még {skill.RemainingCooldown} kör";
                Console.WriteLine($"[1] Attack  [2] Flee  [3] Potion  [4] Skill: {skill.Name} ({skillState})");
                int choice = Input.ReadInt(1, 4);

                bool monsterStrikes;
                switch (choice)
                {
                    case 1:
                        int dealt = monster.TakeDamage(player.RollDamage(random));
                        Console.WriteLine($"Megütöd: {monster.Name} -{dealt} HP.");
                        monsterStrikes = monster.IsAlive;
                        break;
                    case 2:
                        if (!monster.CanFlee)
                        {
                            Console.WriteLine($"{monster.Name} elől nem lehet elmenekülni! (Nem telt el kör.)");
                            continue;
                        }
                        if (random.Next(100) < fleeChance)
                        {
                            Console.WriteLine("Sikerült elmenekülnöd! Jutalom nélkül hagyod el a szobát.");
                            return CombatResult.Fled;
                        }
                        Console.WriteLine("Nem sikerült elmenekülni!");
                        monsterStrikes = true;
                        break;
                    case 4:
                        if (!skill.IsReady)
                        {
                            Console.WriteLine($"A {skill.Name} még {skill.RemainingCooldown} körig nem használható! (Nem telt el kör.)");
                            continue;
                        }
                        Console.WriteLine(skill.Use(player, monster, random));
                        monsterStrikes = monster.IsAlive;
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
                skill.EndRound();

                Console.WriteLine($"-> {player.Name}: {player.Hp} HP, {monster.Name}: {monster.Hp} HP");

                if (!monster.IsAlive)
                {
                    int gold = monster.RollGoldReward(random);
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
