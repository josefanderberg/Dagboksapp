using System;
using System.Linq;
using System.Collections.Generic;

class DiaryMenuHandler
{
    private readonly Diary diary;

    public DiaryMenuHandler(Diary diary)
    {
        this.diary = diary;
    }

    public List<DiaryEntry> GetFilteredEntries(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return diary.GetAllEntries().OrderByDescending(e => e.Date).ToList();
        return diary.GetAllEntries()
            .Where(e =>
                e.Note.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                e.Date.ToString("yyyy-MM-dd").Contains(searchTerm))
            .OrderByDescending(e => e.Date)
            .ToList();
    }

    public void ToggleStar(DiaryEntry entry)
    {
        diary.ToggleStar(entry.Date);
    }

    public void RemoveEntry(DiaryEntry entry)
    {
        diary.RemoveEntry(entry.Date);
    }

    public void UpdateEntry(DiaryEntry entry, string newNote)
    {
        diary.UpdateEntry(entry.Date, newNote);
    }

    public void AddEntry(DiaryEntry entry)
    {
        diary.AddEntry(entry);
    }
}
