namespace TextDungeon
{
    internal class LeaderboardEntry
    {
        public string Name { get; }
        public int Score { get; }
        public Difficulty Difficulty { get; }

        public LeaderboardEntry(string name, int score, Difficulty difficulty)
        {
            Name = name;
            Score = score;
            Difficulty = difficulty;
        }
    }

    /// <summary>
    /// A top 10 eredmény. Csak ez az osztály nyúl a fájlhoz (név;score;difficulty soronként).
    /// </summary>
    internal class Leaderboard
    {
        private const int MaxEntries = 10;
        private readonly string path = Path.Combine(AppContext.BaseDirectory, "leaderboard.txt");
        private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

        /// <summary>Betölti a fájlt. Ha nincs, üres marad; a hibás sorokat átugorja.</summary>
        public void Load()
        {
            entries = new List<LeaderboardEntry>();
            try
            {
                if (!File.Exists(path))
                {
                    return;
                }
                foreach (string line in File.ReadAllLines(path))
                {
                    string[] parts = line.Split(';');
                    if (parts.Length != 3 || string.IsNullOrWhiteSpace(parts[0]))
                    {
                        continue;
                    }
                    if (!int.TryParse(parts[1], out int score) || score < 0)
                    {
                        continue;
                    }
                    if (!Enum.TryParse(parts[2], out Difficulty difficulty) || !Enum.IsDefined(difficulty)
                        || int.TryParse(parts[2], out _))
                    {
                        continue;
                    }
                    entries.Add(new LeaderboardEntry(parts[0], score, difficulty));
                }
                KeepTop();
            }
            catch (Exception)
            {
                Console.WriteLine("A leaderboard fájlt nem sikerült beolvasni, üres listával indulunk.");
                entries = new List<LeaderboardEntry>();
            }
        }

        /// <summary>Felveszi az eredményt, megtartja a top 10-et, és elmenti.</summary>
        public void Add(string name, int score, Difficulty difficulty)
        {
            string safeName = name.Replace(";", "").Trim();
            if (safeName.Length == 0)
            {
                safeName = "Névtelen";
            }
            entries.Add(new LeaderboardEntry(safeName, score, difficulty));
            KeepTop();
            Save();
        }

        public void Print()
        {
            Console.WriteLine("---------- LEADERBOARD ----------");
            if (entries.Count == 0)
            {
                Console.WriteLine("(még nincs eredmény)");
            }
            for (int i = 0; i < entries.Count; i++)
            {
                LeaderboardEntry e = entries[i];
                Console.WriteLine($"{i + 1,2}. {e.Name,-16}{e.Score,7}  {e.Difficulty}");
            }
            Console.WriteLine("---------------------------------");
        }

        private void KeepTop()
        {
            entries = entries.OrderByDescending(e => e.Score).Take(MaxEntries).ToList();
        }

        private void Save()
        {
            try
            {
                File.WriteAllLines(path, entries.Select(e => $"{e.Name};{e.Score};{e.Difficulty}"));
            }
            catch (Exception)
            {
                Console.WriteLine("A leaderboard-ot nem sikerült elmenteni.");
            }
        }
    }
}
