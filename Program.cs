using System;
using System.IO;

class Program
{
    public void Main(string[] args)
    {
        Console.WriteLine("=== DAGBOKSAPP ===");
        Console.WriteLine("1. Skriv ny anteckning");
        Console.WriteLine("2. Lista alla anteckningar");
        Console.WriteLine("3. Sök anteckning på datum");
        Console.WriteLine("4. Spara till fil");
        Console.WriteLine("5. Läs från fil");
        Console.WriteLine("6. Avsluta");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice))

                switch (choice)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;

                    case 6:

                        break;
                    default:
                        break;
                }
        }
    }
}