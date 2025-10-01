using System;
using System.Collections.Generic;

class EntriesMenuUI
{
    private readonly DiaryMenuHandler menuHandler;
    private readonly EntryActionMenuUI entryActionMenu;

    public EntriesMenuUI(DiaryMenuHandler menuHandler, EntryActionMenuUI entryActionMenu)
    {
        this.menuHandler = menuHandler;
        this.entryActionMenu = entryActionMenu;
    }

    public void Show()
    {
        string searchTerm = "";
        while (true)
        {
            List<DiaryEntry> entries = menuHandler.GetFilteredEntries(searchTerm);
            if (entries.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Inga anteckningar hittades.");
                Console.ResetColor();
                return;
            }

            int selected = 0;
            int totalOptions = entries.Count + 2;
            ConsoleKey key;
            do
            {
                Console.Clear();
                ConsoleBoxDrawer.DrawSectionBox("Anteckningar");
                Console.WriteLine("Navigera med piltangenterna. Tryck Enter för att välja.\n");
                for (int i = 0; i < entries.Count; i++)
                {
                    string star = entries[i].IsStarred ? "★" : "";
                    string noteDisplay = entries[i].IsStarred
                        ? $"{star} {entries[i].Note} {star}"
                        : entries[i].Note;

                    if (i == selected)
                    {
                        string line = $"{entries[i].Date:yyyy-MM-dd}: {noteDisplay}";
                        int width = Console.WindowWidth;
                        int toFill = width - line.Length;
                        if (toFill < 0) toFill = 0;

                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(line);
                        Console.ResetColor();
                        if (toFill > 0)
                        {
                            Console.Write(new string(' ', toFill));
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{entries[i].Date:yyyy-MM-dd}: ");
                        Console.ResetColor();
                        Console.WriteLine(noteDisplay);
                    }
                }
                // Sök efter anteckning
                Console.WriteLine();
                string searchOption = "Sök efter anteckning";
                if (selected == entries.Count)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(searchOption);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(searchOption);
                    Console.ResetColor();
                }
                // Tillbaka till menyn
                string backOption = "Tillbaka till menyn...";
                if (selected == entries.Count + 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(backOption);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(backOption);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                    selected = (selected == 0) ? totalOptions - 1 : selected - 1;
                else if (key == ConsoleKey.DownArrow)
                    selected = (selected == totalOptions - 1) ? 0 : selected + 1;
                else if (key == ConsoleKey.Enter)
                {
                    if (selected < entries.Count)
                    {
                        entryActionMenu.Show(entries[selected]);
                        break;
                    }
                    else if (selected == entries.Count)
                    {
                        Console.Write("\nAnge sökterm (lämna tomt för att visa alla): ");
                        Console.CursorVisible = true;
                        searchTerm = Console.ReadLine() ?? "";
                        Console.CursorVisible = false;
                        break;
                    }
                    else if (selected == entries.Count + 1)
                    {
                        return;
                    }
                }
            } while (true);
        }
    }
}
