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

        public bool IsAlive => Hp > 0;

        public Player(string name)
        {
            Name = name;
            MaxHp = 20;
            Hp = MaxHp;
            MinDamage = 2;
            MaxDamage = 5;
            Gold = 0;
            Potions = 1;
        }

        /// <summary>Gyógyít, legfeljebb max HP-ig. Visszaadja, mennyit gyógyult ténylegesen.</summary>
        public int Heal(int amount)
        {
            int before = Hp;
            Hp = Math.Clamp(Hp + amount, 0, MaxHp);
            return Hp - before;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
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
            Console.WriteLine($"HP:       {Hp}/{MaxHp}");
            Console.WriteLine($"Sebzés:   {MinDamage}-{MaxDamage}");
            Console.WriteLine($"Gold:     {Gold}");
            Console.WriteLine($"Potion:   {Potions}");
        }
    }
}
