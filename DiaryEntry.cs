class DiaryEntry
{
    public DateTime Date { get; set; }
    public string Note { get; set; }
    public bool IsStarred { get; set; } // Ny egenskap

    public DiaryEntry(DateTime date, string note, bool isStarred = false)
    {
        Date = date;
        Note = note;
        IsStarred = isStarred;
    }

    public override string ToString()
    {
        string star = IsStarred ? "* " : "";
        return $"{Date:yyyy-MM-dd}: {star}{Note}";
    }
}