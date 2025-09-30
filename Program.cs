using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {

        Diary diary = new Diary();

        while (true)
        {
            Console.WriteLine("=== DAGBOKSAPP ===");
            Console.WriteLine("1. Skriv ny anteckning");
            Console.WriteLine("2. Lista alla anteckningar");
            Console.WriteLine("3. Sök anteckning på datum");
            Console.WriteLine("4. Spara till fil");
            Console.WriteLine("5. Läs från fil");
            Console.WriteLine("6. Avsluta");


            if (int.TryParse(Console.ReadLine(), out int choice))

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Skriv din anteckning:");
                        string note = Console.ReadLine()!;
                        if (string.IsNullOrWhiteSpace(note))
                        {
                            Console.WriteLine("Anteckningen kan inte vara tom.");
                            break;
                        }
                        Console.WriteLine("Skriv datum (yyyy-mm-dd):");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            try
                            {
                                DiaryEntry entry = new DiaryEntry(date, note);
                                diary.AddEntry(entry);
                                Console.WriteLine("Anteckning tillagd!");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt datumformat.");
                        }
                        break;
                    case 2:
                        // List all entries
                        List<DiaryEntry> allEntries = diary.GetAllEntries();
                        if (allEntries.Count == 0)
                        {
                            Console.WriteLine("Inga anteckningar hittades.");
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
                        Console.WriteLine("Ange datum att söka efter (yyyy-mm-dd):");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                        {
                            List<DiaryEntry> entriesByDate = diary.GetEntriesByDate(searchDate);
                            if (entriesByDate.Count == 0)
                            {
                                Console.WriteLine("Inga anteckningar hittades för detta datum.");
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
                            Console.WriteLine("Ogiltigt datumformat.");
                        }
                        break;
                    case 4:
                        try
                        {
                            string filePath = "diary.json";
                            diary.SaveToFile(filePath);
                            Console.WriteLine($"Anteckningar sparade till {filePath}.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ett fel uppstod vid sparning: {ex.Message}");
                        }
                        break;
                    case 5:
                        try
                        {
                            string filePath = "diary.json";
                            diary.LoadFromFile(filePath);
                            Console.WriteLine($"Anteckningar lästa från {filePath}.");
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Filen hittades inte. Skapa och spara anteckningar först.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ett fel uppstod vid läsning: {ex.Message}");
                        }
                        break;
                    case 6:

                        break;
                    default:
                        break;
                }
            else
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
            }
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
