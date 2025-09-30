using System;

class EntryActionMenuUI
{
    private readonly DiaryMenuHandler menuHandler;

    public EntryActionMenuUI(DiaryMenuHandler menuHandler)
    {
        this.menuHandler = menuHandler;
    }

    public void Show(DiaryEntry entry)
    {
        int selected = 0;
        while (true)
        {
            string[] actions = {
                entry.IsStarred ? "Avmarkera stjärna" : "Stjärnmarkera",
                "Ta bort anteckningen",
                "Uppdatera anteckningen"
            };

            Console.Clear();
            ConsoleBoxDrawer.DrawSectionBox("Vald anteckning");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(entry.IsStarred ? $"★ {entry.Note} ★" : entry.Note);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(entry.Date.ToString("yyyy-MM-dd"));
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Vad vill du göra?");
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
                    case 0: // Stjärnmarkera/avmarkera
                        try
                        {
                            menuHandler.ToggleStar(entry);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Fel vid stjärnmarkering: " + ex.Message);
                            Console.ResetColor();
                            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                            Console.ReadKey();
                        }
                        break;
                    case 1: // Ta bort
                        menuHandler.RemoveEntry(entry);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Anteckningen har tagits bort.");
                        Console.ResetColor();
                        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                        return;
                    case 2: // Uppdatera
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
                            menuHandler.UpdateEntry(entry, updatedNote);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Anteckningen har uppdaterats.");
                            Console.ResetColor();
                            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                            Console.ReadKey();
                            return;
                        }
                        break;
                    case 3: // Tillbaka
                        return;
                }
            }
        }
    }
}