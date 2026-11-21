namespace TextDungeon
{
    /// <summary>
    /// Minden konzolos beolvasás itt történik.
    /// </summary>
    internal static class Input
    {
        /// <summary>Beolvas egy sort. Ha a bemenet véget ért (pl. Ctrl+Z), rendben kilép.</summary>
        public static string ReadLine()
        {
            string line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine();
                Console.WriteLine("A bemenet véget ért. Viszlát!");
                Environment.Exit(0);
            }
            return line;
        }

        /// <summary>Nem üres nevet kér be, üres név esetén újra kérdez.</summary>
        public static string ReadName(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string name = ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
                Console.WriteLine("A név nem lehet üres, próbáld újra!");
            }
        }

        /// <summary>Vár egy billentyűre (átirányított bemenetnél nem).</summary>
        public static void WaitForKey()
        {
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
        }
    }
}
