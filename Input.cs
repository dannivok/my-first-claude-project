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
            return line.Replace("﻿", ""); // átirányított bemenet elején lehet BOM
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

        /// <summary>
        /// Az egyetlen számbeolvasó metódus. Csak min és max közötti számmal tér vissza,
        /// hibás bevitelnél szól és újra kérdez.
        /// </summary>
        public static int ReadInt(int min, int max)
        {
            while (true)
            {
                Console.Write($"Választás ({min}-{max}): ");
                string input = ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Nem írtál be semmit, próbáld újra!");
                }
                else if (!int.TryParse(input.Trim(), out int number))
                {
                    Console.WriteLine("Ez nem szám, próbáld újra!");
                }
                else if (number < min || number > max)
                {
                    Console.WriteLine($"Csak {min} és {max} közötti számot választhatsz!");
                }
                else
                {
                    return number;
                }
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
