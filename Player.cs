namespace TextDungeon
{
    /// <summary>
    /// A játékos minden adata: név, HP, max HP, sebzés, gold, potion.
    /// A kasztok közös absztrakt őse: a kezdő statokat a kaszt adja meg,
    /// a passzív képességet egy virtuális metódus felülírása.
    /// </summary>
    internal abstract class Player
    {
        public abstract string ClassName { get; }
        public abstract string PassiveDescription { get; }

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

        public ISkill Skill { get; }

        protected Player(string name, DifficultySettings settings, int baseHp, int minDamage, int maxDamage, ISkill skill)
        {
            Name = name;
            Skill = skill;
            MaxHp = Math.Max(1, baseHp + settings.HpBonus);
            Hp = MaxHp;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Gold = 0;
            AddGold(settings.StartGold);
            Potions = 1;
        }

        /// <summary>A kapott sebzés módosítása (alapból nincs).</summary>
        protected virtual int ReduceDamage(int amount)
        {
            return amount;
        }

        /// <summary>Kitér-e egy támadás elől (alapból soha).</summary>
        public virtual bool TryDodge(Random random)
        {
            return false;
        }

        /// <summary>Új szobába lépéskor fut le. Ha történt valami, leírja, különben null.</summary>
        public virtual string OnEnterRoom()
        {
            return null;
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
            amount = ReduceDamage(amount);
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
            Console.WriteLine($"Kaszt:    {ClassName} ({PassiveDescription})");
            Console.WriteLine($"Skill:    {Skill.Name} ({Skill.Description}, 3 kör cooldown)");
            Console.WriteLine($"Level:    {Level}");
            Console.WriteLine($"XP:       {Xp}/{XpToNextLevel}");
            Console.WriteLine($"HP:       {Hp}/{MaxHp}");
            Console.WriteLine($"Sebzés:   {MinDamage}-{MaxDamage}");
            Console.WriteLine($"Gold:     {Gold}");
            Console.WriteLine($"Potion:   {Potions}");
        }
    }
}
