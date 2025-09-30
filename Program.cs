using System.IO;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== DAGBOKSAPP ===");
        Console.WriteLine("1. Skriv ny anteckning");
        Console.WriteLine("2. Lista alla anteckningar");
        Console.WriteLine("3. Sök anteckning på datum");
        Console.WriteLine("4. Spara till fil");
        Console.WriteLine("5. Läs från fil");
        Console.WriteLine("6. Avsluta");

        while (true)
        {
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
                            DiaryEntry entry = new DiaryEntry(date, note);
                            Diary diary = new Diary();
                            diary.AddEntry(entry);
                            Console.WriteLine("Anteckning tillagd!");
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt datumformat.");
                        }
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;

                    case 6:

                        break;
                    default:
                        break;
                }
        }
    }
}

class DiaryEntry
{
    public DateTime Date { get; set; }
    public string Note { get; set; }

    public DiaryEntry(DateTime date, string note)
    {
        Date = date;
        Note = note;
    }
    public override string ToString()
    {
        return $"{Date.ToShortDateString()}: {Note}";
    }
}

class Diary
{
    private List<DiaryEntry> entries = new List<DiaryEntry>();

    public void AddEntry(DiaryEntry entry)
    {
        entries.Add(entry);
    }

    

}