using System;
using System.Linq;

class DiaryMenuHandler
{
    public static void HandleEntriesMenu(Diary diary)
    {
        Console.CursorVisible = false;
        try
        {
            string searchTerm = "";
            while (true)
            {
                // Filtrera anteckningar om sökterm finns, annars visa alla
                List<DiaryEntry> allEntries = string.IsNullOrWhiteSpace(searchTerm)
                    ? diary.GetAllEntries().OrderByDescending(e => e.Date).ToList()
                    : diary.GetAllEntries()
                        .Where(e =>
                            e.Note.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            e.Date.ToString("yyyy-MM-dd").Contains(searchTerm))
                        .OrderByDescending(e => e.Date)
                        .ToList();

                if (allEntries.Count == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Inga anteckningar hittades.");
                    Console.ResetColor();

                    return;
                }

                int selected = 0;
                int totalOptions = allEntries.Count + 2; // +2 för "Sök efter anteckning" och "Tillbaka till menyn..."
                ConsoleKey key;
                do
                {
                    Console.Clear();
                    Program.DrawSectionBox("Lista och hantera anteckningar"); // Ny ruta
                    Console.WriteLine("Navigera med piltangenterna. Tryck Enter för att välja.\n");
                    for (int i = 0; i < allEntries.Count; i++)
                    {
                        string starPrefix = allEntries[i].IsStarred ? "★ " : "";
                        string starSuffix = allEntries[i].IsStarred ? " *" : "";
                        if (i == selected)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{allEntries[i].Date:yyyy-MM-dd}: {starPrefix}{allEntries[i].Note}{starSuffix}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"{allEntries[i].Date:yyyy-MM-dd}: ");
                            Console.ResetColor();
                            Console.WriteLine($"{starPrefix}{allEntries[i].Note}{starSuffix}");
                        }
                    }
                    // Sök efter anteckning
                    Console.WriteLine();
                    if (selected == allEntries.Count)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Sök efter anteckning");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; // Cyanblå färg
                        Console.WriteLine("Sök efter anteckning");
                        Console.ResetColor();
                    }


                    // Tillbaka till menyn
                    if (selected == allEntries.Count + 1)
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
                        if (selected < allEntries.Count)
                        {
                            HandleEntryAction(diary, allEntries[selected]);
                            break;
                        }
                        else if (selected == allEntries.Count)
                        {
                            // Sök efter anteckning
                            Console.ResetColor();
                            Console.Write("\nAnge sökterm (lämna tomt för att visa alla): ");
                            Console.CursorVisible = true;
                            searchTerm = Console.ReadLine() ?? "";
                            Console.CursorVisible = false;
                            break;
                        }
                        else if (selected == allEntries.Count + 1)
                        {
                            return; // Tillbaka till menyn
                        }
                    }
                } while (true);
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    public static void HandleEntryAction(Diary diary, DiaryEntry entry)
    {
        Console.CursorVisible = false;
        string[] actions = {
            entry.IsStarred ? "Avmarkera stjärna" : "Stjärnmarkera", // Nytt val
            "Ta bort anteckningen",
            "Uppdatera anteckningen"
        };
        int selected = 0;
        try
        {
            while (true)
            {
                Console.Clear();
                Program.DrawSectionBox("Vald anteckning"); // Ny ruta

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(entry.Note);
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
                            diary.ToggleStar(entry.Date);
                            entry.IsStarred = !entry.IsStarred;
                            return;
                        case 1: // Ta bort
                            diary.RemoveEntry(entry.Date);
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
                                diary.UpdateEntry(entry.Date, updatedNote);
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
        finally
        {
            Console.CursorVisible = true;
        }
    }
}
