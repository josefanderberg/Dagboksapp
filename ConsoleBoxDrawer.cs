
using System;

static class ConsoleBoxDrawer
{
    public static void DrawSectionBox(string title)
    {
        int boxWidth = 32;
        string horizontal = "┌" + new string('─', boxWidth - 2) + "┐";
        string bottom = "└" + new string('─', boxWidth - 2) + "┘";
        int pad = (boxWidth - 2 - title.Length) / 2;
        string middle = "│" + new string(' ', pad) + title + new string(' ', boxWidth - 2 - pad - title.Length) + "│";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(horizontal);
        Console.WriteLine(middle);
        Console.WriteLine(bottom);
        Console.ResetColor();
    }
}