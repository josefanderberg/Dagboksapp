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
                        DrawSectionBox("Skriv ny anteckning"); // Ny ruta
                        Console.WriteLine();
                        Console.Write("Skriv din anteckning: ");
                        string note = Console.ReadLine()!;
                        if (string.IsNullOrWhiteSpace(note))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Anteckningen kan inte vara tom.");
                            Console.ResetColor();
                            break;
                        }

                        // Nytt sätt att skriva datum
                        string dateTemplate = "yyyy-mm-dd";
                        string inputDate = dateTemplate;
                        int cursorLeft = "Skriv datum: ".Length;
                        Console.Write("Skriv datum: ");
                        Console.Write(inputDate);
                        int pos = 0;
                        ConsoleKeyInfo keyInfo;
                        Console.CursorVisible = true;
                        while (pos < inputDate.Length)
                        {
                            Console.SetCursorPosition(cursorLeft + pos, Console.CursorTop);
                            keyInfo = Console.ReadKey(true);
                            if (char.IsDigit(keyInfo.KeyChar) && (pos == 0 || pos == 1 || pos == 2 || pos == 3 || pos == 5 || pos == 6 || pos == 8 || pos == 9))
                            {
                                inputDate = inputDate.Remove(pos, 1).Insert(pos, keyInfo.KeyChar.ToString());
                                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                                Console.Write(inputDate);
                                pos++;
                                // Hoppa över '-' automatiskt
                                if (pos == 4 || pos == 7) pos++;
                            }
                            else if (keyInfo.Key == ConsoleKey.LeftArrow && pos > 0)
                            {
                                pos--;
                                if (pos == 4 || pos == 7) pos--;
                            }
                            else if (keyInfo.Key == ConsoleKey.RightArrow && pos < inputDate.Length - 1)
                            {
                                pos++;
                                if (pos == 4 || pos == 7) pos++;
                            }
                            else if (keyInfo.Key == ConsoleKey.Backspace && pos > 0)
                            {
                                pos--;
                                if (pos == 4 || pos == 7) pos--;
                                inputDate = inputDate.Remove(pos, 1).Insert(pos, dateTemplate[pos].ToString());
                                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                                Console.Write(inputDate);
                            }
                            else if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                        Console.CursorVisible = false;
                        Console.WriteLine();

                        if (DateTime.TryParse(inputDate, out DateTime date))
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

    public static void DrawTitleBox(string title)
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

    public static void DrawSectionBox(string title)
    {
        int boxWidth = 32;
        string horizontal = "┌" + new string('─', boxWidth - 2) + "┐";
        string bottom =    "└" + new string('─', boxWidth - 2) + "┘";
        int pad = (boxWidth - 2 - title.Length) / 2;
        string middle = "│" + new string(' ', pad) + title + new string(' ', boxWidth - 2 - pad - title.Length) + "│";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(horizontal);
        Console.WriteLine(middle);
        Console.WriteLine(bottom);
        Console.ResetColor();
    }
}
