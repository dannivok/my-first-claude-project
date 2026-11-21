namespace TextDungeon
{
    internal enum ShopEffect
    {
        MaxHp,
        Damage,
        Potion,
    }

    /// <summary>Egy shop tétel: név, ár és hogy mit ad.</summary>
    internal class ShopItem
    {
        public string Name { get; }
        public int Price { get; }
        public ShopEffect Effect { get; }

        public ShopItem(string name, int price, ShopEffect effect)
        {
            Name = name;
            Price = price;
            Effect = effect;
        }

        public void Apply(Player player)
        {
            switch (Effect)
            {
                case ShopEffect.MaxHp:
                    player.IncreaseMaxHp(5);
                    break;
                case ShopEffect.Damage:
                    player.IncreaseDamage(1);
                    break;
                case ShopEffect.Potion:
                    player.AddPotion();
                    break;
            }
        }
    }

    internal class Shop
    {
        private readonly List<ShopItem> items = new List<ShopItem>
        {
            new ShopItem("+5 max HP", 20, ShopEffect.MaxHp),
            new ShopItem("+1 sebzés", 25, ShopEffect.Damage),
            new ShopItem("Potion", 10, ShopEffect.Potion),
        };

        public void Open(Player player)
        {
            Console.WriteLine();
            Console.WriteLine("=== SHOP ===");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Gold: {player.Gold} | HP {player.Hp}/{player.MaxHp} | Sebzés {player.MinDamage}-{player.MaxDamage} | Potion {player.Potions}");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {items[i].Name,-12}{items[i].Price,5} gold");
                }
                Console.WriteLine("[0] Kilépés a shop-ból");

                int choice = Input.ReadInt(0, items.Count);
                if (choice == 0)
                {
                    Console.WriteLine("Elhagyod a shop-ot.");
                    return;
                }

                ShopItem item = items[choice - 1];
                if (!player.SpendGold(item.Price))
                {
                    Console.WriteLine($"Nincs elég gold-od! ({item.Name}: {item.Price} gold, neked {player.Gold} van)");
                    continue;
                }
                item.Apply(player);
                Console.WriteLine($"Megvetted: {item.Name}.");
            }
        }
    }
}
