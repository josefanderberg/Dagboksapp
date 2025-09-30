using System.Text.Json;

class Diary
{
    private List<DiaryEntry> entries = new List<DiaryEntry>();

    public void AddEntry(DiaryEntry entry)
    {
        if (entries.Any(e => e.Date.Date == entry.Date.Date))
        {
            throw new InvalidOperationException("Det finns redan en anteckning för detta datum.");
        }
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
    //Spara i json fil
    public void SaveToFile(string filePath)
    {
        string json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void LoadFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath);
        entries = JsonSerializer.Deserialize<List<DiaryEntry>>(json) ?? new List<DiaryEntry>();
    }
}