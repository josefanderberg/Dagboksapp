using System;

class MainMenuUI
{
    private readonly EntriesMenuUI entriesMenu;
    private readonly AddEntryUI addEntryUI;
    private readonly Diary diary;

    public MainMenuUI(EntriesMenuUI entriesMenu, AddEntryUI addEntryUI, Diary diary)
    {
        this.entriesMenu = entriesMenu;
        this.addEntryUI = addEntryUI;
        this.diary = diary;
    }

    public void Show(string infoMessage = "")
    {
        while (true)
        {
            Console.Clear();
            ConsoleBoxDrawer.DrawSectionBox("DAGBOKSAPP");
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
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;
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
                        addEntryUI.Show();
                        infoMessage = "";
                        break;
                    case 2:
                        entriesMenu.Show();
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
}
