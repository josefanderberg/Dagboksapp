using System.IO;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        DiaryService diaryService = new DiaryService();
        string infoMessage;

        // Försök ladda anteckningar direkt vid start
        try
        {
            string filePath = "diary.json";
            diaryService.LoadFromFile(filePath);
            infoMessage = $"Anteckningar lästa från {filePath}.";
        }
        catch (FileNotFoundException ex)
        {
            Logger.LogError(ex);
            infoMessage = "Filen hittades inte. Skapa och spara anteckningar först.";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            infoMessage = $"Ett fel uppstod vid läsning: {ex.Message}";
        }

        var menuHandler = new DiaryMenuHandler(diaryService);
        var ui = new DiaryConsoleUI(menuHandler, diaryService);
        ui.ShowMainMenu(infoMessage);
    }
}
