namespace TextDungeon
{
    internal class Warrior : Player
    {
        public Warrior(string name, DifficultySettings settings) : base(name, settings, 26, 3, 5) { }

        public override string ClassName => "Warrior";
        public override string PassiveDescription => "minden kapott sebzésből 1-gyel kevesebbet veszít, de legalább 1-et";

        protected override int ReduceDamage(int amount)
        {
            return Math.Max(1, amount - 1);
        }
    }

    internal class Mage : Player
    {
        public Mage(string name, DifficultySettings settings) : base(name, settings, 16, 2, 4) { }

        public override string ClassName => "Mage";
        public override string PassiveDescription => "minden új szobába lépéskor 1 HP-t regenerál";

        public override string OnEnterRoom()
        {
            int healed = Heal(1);
            return healed > 0 ? $"A mágiád regenerál: +{healed} HP." : null;
        }
    }

    internal class Rogue : Player
    {
        public Rogue(string name, DifficultySettings settings) : base(name, settings, 20, 2, 5) { }

        public override string ClassName => "Rogue";
        public override string PassiveDescription => "20% eséllyel dodge-ol egy támadást";

        public override bool TryDodge(Random random)
        {
            return random.Next(100) < 20;
        }
    }
}
