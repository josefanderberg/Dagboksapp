using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        Diary diary = new Diary();
        string infoMessage = "";

        // Försök ladda anteckningar direkt vid start
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

        while (true)
        {
            Console.Clear();
            DrawTitleBox("DAGBOKSAPP");
            Console.WriteLine();
            Console.WriteLine("1. Skriv ny anteckning");
            Console.WriteLine("2. Lista och hantera anteckningar");
            Console.WriteLine("3. Ladda");
            Console.WriteLine("4. Spara");
            Console.WriteLine("5. Avsluta");
            Console.WriteLine();

            if (!string.IsNullOrWhiteSpace(infoMessage))
            {
                if (infoMessage.StartsWith("Anteckningar lästa från") || infoMessage.StartsWith("Anteckningar sparade till"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray; // Ändrad från DarkGreen till DarkGray
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine(infoMessage);
                Console.ResetColor();
                Console.WriteLine();
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
                        DiaryMenuHandler.HandleEntriesMenu(diary);
                        infoMessage = "";
                        break;
                    case 3:
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

    static void DrawTitleBox(string title)
    {
        int boxWidth = 32;
        string horizontal = "┌" + new string('─', boxWidth - 2) + "┐";
        string bottom =    "└" + new string('─', boxWidth - 2) + "┘";
        int pad = (boxWidth - 2 - title.Length) / 2;
        string middle = "│" + new string(' ', pad) + title + new string(' ', boxWidth - 2 - pad - title.Length) + "│";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(horizontal);
        Console.WriteLine(middle);
        Console.WriteLine(bottom);
        Console.ResetColor();
    }
}
