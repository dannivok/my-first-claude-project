namespace TextDungeon
{
    /// <summary>
    /// A játékos minden adata: név, HP, max HP, sebzés, gold, potion.
    /// </summary>
    internal class Player
    {
        public string Name { get; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int Gold { get; private set; }
        public int Potions { get; private set; }
        /// <summary>Az összes valaha összegyűjtött gold (a kezdő és az elköltött is).</summary>
        public int TotalGoldEarned { get; private set; }
        public int Level { get; private set; } = 1;
        public int Xp { get; private set; }

        /// <summary>Az 1. level up 10, a 2. 20, a 3. 30 XP-nél jön (és így tovább).</summary>
        public int XpToNextLevel => 10 * Level;

        public bool IsAlive => Hp > 0;

        public Player(string name, DifficultySettings settings)
        {
            Name = name;
            MaxHp = 20 + settings.HpBonus;
            Hp = MaxHp;
            MinDamage = 2;
            MaxDamage = 5;
            Gold = 0;
            AddGold(settings.StartGold);
            Potions = 1;
        }

        /// <summary>Gyógyít, legfeljebb max HP-ig. Visszaadja, mennyit gyógyult ténylegesen.</summary>
        public int Heal(int amount)
        {
            int before = Hp;
            Hp = Math.Clamp(Hp + amount, 0, MaxHp);
            return Hp - before;
        }

        /// <summary>Sebzést kap, a HP 0 alá nem mehet. Visszaadja a ténylegesen elvesztett HP-t.</summary>
        public int TakeDamage(int amount)
        {
            int before = Hp;
            Hp = Math.Clamp(Hp - amount, 0, MaxHp);
            return before - Hp;
        }

        public int RollDamage(Random random)
        {
            return random.Next(MinDamage, MaxDamage + 1);
        }

        /// <summary>XP-t ad. Visszaadja, hány level up történt.</summary>
        public int AddXp(int amount)
        {
            Xp += amount;
            int levelUps = 0;
            while (Xp >= XpToNextLevel)
            {
                Xp -= XpToNextLevel;
                Level++;
                MaxHp += 3;
                MinDamage++;
                MaxDamage++;
                Hp = MaxHp;
                levelUps++;
            }
            return levelUps;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            TotalGoldEarned += amount;
        }

        /// <summary>Levonja az árat, ha van elég gold. Ha nincs, false.</summary>
        public bool SpendGold(int amount)
        {
            if (Gold < amount)
            {
                return false;
            }
            Gold -= amount;
            return true;
        }

        public void IncreaseMaxHp(int amount)
        {
            MaxHp += amount;
            Heal(amount);
        }

        /// <summary>A sebzéstartomány mindkét végét növeli.</summary>
        public void IncreaseDamage(int amount)
        {
            MinDamage += amount;
            MaxDamage += amount;
        }

        public void AddPotion()
        {
            Potions++;
        }

        /// <summary>Megiszik egy potion-t (+8 HP). Ha nincs potion, false.</summary>
        public bool DrinkPotion()
        {
            if (Potions <= 0)
            {
                return false;
            }
            Potions--;
            Heal(8);
            return true;
        }

        public void PrintStats()
        {
            Console.WriteLine($"Név:      {Name}");
            Console.WriteLine($"Level:    {Level}");
            Console.WriteLine($"XP:       {Xp}/{XpToNextLevel}");
            Console.WriteLine($"HP:       {Hp}/{MaxHp}");
            Console.WriteLine($"Sebzés:   {MinDamage}-{MaxDamage}");
            Console.WriteLine($"Gold:     {Gold}");
            Console.WriteLine($"Potion:   {Potions}");
        }
    }
}
