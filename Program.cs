using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        Diary diary = new Diary();
        string infoMessage;

        // Försök ladda anteckningar direkt vid start
        try
        {
            string filePath = "diary.json";
            diary.LoadFromFile(filePath);
            infoMessage = $"Anteckningar lästa från {filePath}.";
        }
        catch (FileNotFoundException)
        {
            infoMessage = "Filen hittades inte. Skapa och spara anteckningar först.";
        }
        catch (Exception ex)
        {
            infoMessage = $"Ett fel uppstod vid läsning: {ex.Message}";
        }

        var menuHandler = new DiaryMenuHandler(diary);
        var ui = new DiaryConsoleUI(menuHandler, diary);
        ui.ShowMainMenu(infoMessage);
    }
}
