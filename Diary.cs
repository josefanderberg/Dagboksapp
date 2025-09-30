class Diary
{
    private List<DiaryEntry> entries = new List<DiaryEntry>();

    public void AddEntry(DiaryEntry entry)
    {
        entries.Add(entry);
    }
    public List<DiaryEntry> GetAllEntries()
    {
        return entries;
    }
    public List<DiaryEntry> GetEntriesByDate(DateTime date)
    {
        return entries.Where(e => e.Date.Date == date.Date).ToList();
    }

}