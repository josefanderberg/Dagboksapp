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

