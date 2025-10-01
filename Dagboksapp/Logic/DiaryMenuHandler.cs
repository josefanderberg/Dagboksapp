using System;
using System.Linq;
using System.Collections.Generic;

class DiaryMenuHandler
{
    private readonly DiaryService diaryService;

    public DiaryMenuHandler(DiaryService diaryService)
    {
        this.diaryService = diaryService;
    }

    public List<DiaryEntry> GetFilteredEntries(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return diaryService.GetAllEntries().OrderByDescending(e => e.Date).ToList();
        return diaryService.GetAllEntries()
            .Where(e =>
                e.Note.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                e.Date.ToString("yyyy-MM-dd").Contains(searchTerm))
            .OrderByDescending(e => e.Date)
            .ToList();
    }

    public void ToggleStar(DiaryEntry entry)
    {
        diaryService.ToggleStar(entry.Date);
    }

    public void RemoveEntry(DiaryEntry entry)
    {
        diaryService.RemoveEntry(entry.Date);
    }

    public void UpdateEntry(DiaryEntry entry, string newNote)
    {
        diaryService.UpdateEntry(entry.Date, newNote);
    }

    public void AddEntry(DiaryEntry entry)
    {
        diaryService.AddEntry(entry);
    }
}
