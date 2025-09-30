using System;
using System.Collections.Generic;

class DiaryConsoleUI
{
    private readonly MainMenuUI mainMenu;

    private readonly AddEntryUI addEntryUI;

    private readonly EntriesMenuUI entriesMenu;
    private readonly EntryActionMenuUI entryActionMenu;

    public DiaryConsoleUI(DiaryMenuHandler menuHandler, Diary diary)
    {
        entryActionMenu = new EntryActionMenuUI(menuHandler);
        entriesMenu = new EntriesMenuUI(menuHandler, entryActionMenu);
        addEntryUI = new AddEntryUI(menuHandler);
        mainMenu = new MainMenuUI(entriesMenu, addEntryUI, diary);
    }

    public void ShowMainMenu(string infoMessage = "")
    {
        mainMenu.Show(infoMessage);
    }
}
