using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        Diary diary = new Diary();

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===================================");
            Console.WriteLine("          DAGBOKSAPP");
            Console.WriteLine("===================================");
            Console.ResetColor();
            Console.WriteLine("1. Skriv ny anteckning");
            Console.WriteLine("2. Lista alla anteckningar");
            Console.WriteLine("3. Sök anteckning på datum");
            Console.WriteLine("4. Ta bort eller uppdatera en anteckning");
            Console.WriteLine("5. Spara till fil");
            Console.WriteLine("6. Läs från fil");
            Console.WriteLine("7. Avsluta");
            Console.WriteLine("===================================");

            Console.Write("Välj ett alternativ: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Skriv ny anteckning ===");
                        Console.ResetColor();
                        Console.Write("Skriv din anteckning: ");
                        string note = Console.ReadLine()!;
                        if (string.IsNullOrWhiteSpace(note))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Anteckningen kan inte vara tom.");
                            Console.ResetColor();
                            break;
                        }
                        Console.Write("Skriv datum (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            try
                            {
                                DiaryEntry entry = new DiaryEntry(date, note);
                                diary.AddEntry(entry);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Anteckning tillagd!");
                                Console.ResetColor();
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ogiltigt datumformat.");
                            Console.ResetColor();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Lista alla anteckningar ===");
                        Console.ResetColor();
                        List<DiaryEntry> allEntries = diary.GetAllEntries();
                        if (allEntries.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Inga anteckningar hittades.");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (var entry in allEntries)
                            {
                                Console.WriteLine(entry);
                            }
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Sök anteckning på datum ===");
                        Console.ResetColor();
                        Console.Write("Ange datum att söka efter (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                        {
                            List<DiaryEntry> entriesByDate = diary.GetEntriesByDate(searchDate);
                            if (entriesByDate.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Inga anteckningar hittades för detta datum.");
                                Console.ResetColor();
                            }
                            else
                            {
                                foreach (var entry in entriesByDate)
                                {
                                    Console.WriteLine(entry);
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ogiltigt datumformat.");
                            Console.ResetColor();
                        }
                        break;
                    case 4:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Ta bort eller uppdatera en anteckning ===");
                        Console.ResetColor();
                        Console.Write("Ange datum för anteckningen (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime targetDate))
                        {
                            List<DiaryEntry> entriesByDate = diary.GetEntriesByDate(targetDate);
                            if (entriesByDate.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Ingen anteckning hittades för detta datum.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Clear();
                                DiaryEntry selectedEntry = entriesByDate.First();
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"Vald anteckning: {selectedEntry}");
                                Console.ResetColor();
                                Console.WriteLine("Vad vill du göra?");
                                Console.WriteLine("1. Ta bort anteckningen");
                                Console.WriteLine("2. Uppdatera anteckningen");
                                if (int.TryParse(Console.ReadLine(), out int actionChoice))
                                {
                                    switch (actionChoice)
                                    {
                                        case 1:
                                            diary.RemoveEntry(targetDate);
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Anteckningen har tagits bort.");
                                            Console.ResetColor();
                                            break;
                                        case 2:
                                            Console.Write("Skriv den nya anteckningen: ");
                                            string updatedNote = Console.ReadLine()!;
                                            if (string.IsNullOrWhiteSpace(updatedNote))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("Anteckningen kan inte vara tom.");
                                                Console.ResetColor();
                                            }
                                            else
                                            {
                                                diary.UpdateEntry(targetDate, updatedNote);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("Anteckningen har uppdaterats.");
                                                Console.ResetColor();
                                            }
                                            break;
                                        default:
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Ogiltigt val.");
                                            Console.ResetColor();
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Ogiltigt val.");
                                    Console.ResetColor();
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ogiltigt datumformat.");
                            Console.ResetColor();
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Spara till fil ===");
                        Console.ResetColor();
                        try
                        {
                            string filePath = "diary.json";
                            diary.SaveToFile(filePath);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Anteckningar sparade till {filePath}.");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ett fel uppstod vid sparning: {ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("=== Läs från fil ===");
                        Console.ResetColor();
                        try
                        {
                            string filePath = "diary.json";
                            diary.LoadFromFile(filePath);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Anteckningar lästa från {filePath}.");
                            Console.ResetColor();
                        }
                        catch (FileNotFoundException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Filen hittades inte. Skapa och spara anteckningar först.");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ett fel uppstod vid läsning: {ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case 7:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Tack för att du använde Dagboksappen!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        Console.ResetColor();
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val, försök igen.");
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
