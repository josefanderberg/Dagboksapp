using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        Diary diary = new Diary();
        string infoMessage = "";

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===================================");
            Console.WriteLine("          DAGBOKSAPP");
            Console.WriteLine("===================================");
            Console.ResetColor();

            Console.WriteLine("1. Skriv ny anteckning");
            Console.WriteLine("2. Lista och hantera anteckningar");
            Console.WriteLine("3. Sök anteckning på datum");
            Console.WriteLine("4. Spara");
            Console.WriteLine("5. Ladda");
            Console.WriteLine("6. Avsluta");
            Console.WriteLine("===================================");

            if (!string.IsNullOrWhiteSpace(infoMessage))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(infoMessage);
                Console.ResetColor();
                Console.WriteLine("===================================");
            }

            Console.Write("Välj ett alternativ: ");
            bool skipPause = false;
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
                        infoMessage = "";
                        break;
                    case 2:
                        HandleEntriesMenu(diary);
                        infoMessage = "";
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
                                    Console.WriteLine(entry); // Alla Console.WriteLine(entry) använder nu ToString() från DiaryEntry och visar datum som yyyy-MM-dd.
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ogiltigt datumformat.");
                            Console.ResetColor();
                        }
                        infoMessage = "";
                        break;
                    case 4:
                        try
                        {
                            string filePath = "diary.json";
                            diary.SaveToFile(filePath);
                            infoMessage = $"Anteckningar sparade till {filePath}.";
                        }
                        catch (Exception ex)
                        {
                            infoMessage = $"Ett fel uppstod vid sparning: {ex.Message}";
                        }
                        skipPause = true;
                        break;
                    case 5:
                        try
                        {
                            string filePath = "diary.json";
                            diary.LoadFromFile(filePath);
                            infoMessage = $"Anteckningar lästa från {filePath}.";
                        }
                        catch (FileNotFoundException)
                        {
                            infoMessage = "Filen hittades inte. Skapa och spara anteckningar först.";
                        }
                        catch (Exception ex)
                        {
                            infoMessage = $"Ett fel uppstod vid läsning: {ex.Message}";
                        }
                        skipPause = true;
                        break;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Tack för att du använde Dagboksappen!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        Console.ResetColor();
                        infoMessage = "";
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val, försök igen.");
                Console.ResetColor();
                infoMessage = "";
            }
            if (!skipPause)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }

    static void HandleEntriesMenu(Diary diary)
    {
        Console.CursorVisible = false;
        try
        {
            while (true)
            {
                List<DiaryEntry> allEntries = diary.GetAllEntries()
                    .OrderByDescending(e => e.Date)
                    .ToList();
                if (allEntries.Count == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Inga anteckningar hittades.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                int selected = 0;
                int totalOptions = allEntries.Count + 1; // +1 för "Tillbaka till menyn..."
                ConsoleKey key;
                do
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=== Lista och hantera anteckningar ===");
                    Console.ResetColor();
                    Console.WriteLine("Navigera med piltangenterna. Tryck Enter för att välja.\n");
                    for (int i = 0; i < allEntries.Count; i++)
                    {
                        if (i == selected)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.Write(allEntries[i]);
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                    // Tom rad före "Tillbaka till menyn..."
                    Console.WriteLine();
                    if (selected == allEntries.Count)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.WriteLine("Tillbaka till menyn...");
                    Console.ResetColor();

                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.UpArrow)
                        selected = (selected == 0) ? totalOptions - 1 : selected - 1;
                    else if (key == ConsoleKey.DownArrow)
                        selected = (selected == totalOptions - 1) ? 0 : selected + 1;
                    else if (key == ConsoleKey.Enter)
                    {
                        if (selected == allEntries.Count)
                            return; // Tillbaka till menyn, ingen extra prompt
                        HandleEntryAction(diary, allEntries[selected]);
                        break;
                    }
                } while (true);
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    static void HandleEntryAction(Diary diary, DiaryEntry entry)
    {
        Console.CursorVisible = false;
        string[] actions = { "Ta bort anteckningen", "Uppdatera anteckningen" };
        int selected = 0;
        try
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Vald anteckning ===");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(entry.Note);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(entry.Date.ToString("yyyy-MM-dd"));
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("> Vad vill du göra?");
                Console.WriteLine();

                for (int i = 0; i < actions.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(actions[i]);
                    Console.ResetColor();
                }
                // "Tillbaka" längst ner, i grått och markerbar
                if (selected == actions.Length)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine();
                Console.WriteLine("Tillbaka till menyn...");
                Console.ResetColor();

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                    selected = (selected == 0) ? actions.Length : selected - 1;
                else if (key == ConsoleKey.DownArrow)
                    selected = (selected == actions.Length) ? 0 : selected + 1;
                else if (key == ConsoleKey.Enter)
                {
                    switch (selected)
                    {
                        case 0: // Ta bort
                            diary.RemoveEntry(entry.Date);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Anteckningen har tagits bort.");
                            Console.ResetColor();
                            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                            Console.ReadKey();
                            return;
                        case 1: // Uppdatera
                            Console.Write("Skriv den nya anteckningen: ");
                            string updatedNote = Console.ReadLine()!;
                            if (string.IsNullOrWhiteSpace(updatedNote))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Anteckningen kan inte vara tom.");
                                Console.ResetColor();
                                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                                Console.ReadKey();
                            }
                            else
                            {
                                diary.UpdateEntry(entry.Date, updatedNote);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Anteckningen har uppdaterats.");
                                Console.ResetColor();
                                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                                Console.ReadKey();
                                return;
                            }
                            break;
                        case 2: // Tillbaka
                            return;
                    }
                }
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }
}
