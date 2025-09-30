using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

class UpgradeJson
{
    public static void Upgrade(string filePath)
    {
        string json = File.ReadAllText(filePath);
        var jsonArray = JsonNode.Parse(json)?.AsArray();

        if (jsonArray == null || jsonArray.Count == 0)
        {
            Console.WriteLine("Inga anteckningar att uppgradera.");
            return;
        }

        bool needsUpgrade = false;

        foreach (var item in jsonArray)
        {
            if (item is JsonObject obj)
            {
                if (!obj.ContainsKey("IsStarred"))
                {
                    obj["IsStarred"] = false;
                    needsUpgrade = true;
                }
            }
        }

        if (needsUpgrade)
        {
            string upgradedJson = jsonArray.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, upgradedJson);
            Console.WriteLine("Anteckningar uppgraderade med 'IsStarred' fält.");
        }
        else
        {
            Console.WriteLine("Alla anteckningar är redan uppgraderade.");
        }
    }
}