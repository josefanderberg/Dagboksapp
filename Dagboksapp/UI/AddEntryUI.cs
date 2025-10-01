using System;

class AddEntryUI
{
    private readonly DiaryMenuHandler menuHandler;

    public AddEntryUI(DiaryMenuHandler menuHandler)
    {
        this.menuHandler = menuHandler;
    }

    public void Show()
    {
        Console.Clear();
        ConsoleBoxDrawer.DrawSectionBox("Skriv ny anteckning");
        Console.WriteLine();
        Console.Write("Skriv din anteckning: ");
        string note = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(note))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Anteckningen kan inte vara tom.");
            Console.ResetColor();
            return;
        }

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
            if (date > DateTime.Today)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt datum: Du kan inte lägga till en anteckning i framtiden.");
                Console.ResetColor();
                return;
            }
            try
            {
                menuHandler.AddEntry(new DiaryEntry(date, note));
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
            var ex = new FormatException("Ogiltigt datumformat: " + inputDate);
            Logger.LogError(ex);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt datumformat.");
            Console.ResetColor();
        }
    }
}
