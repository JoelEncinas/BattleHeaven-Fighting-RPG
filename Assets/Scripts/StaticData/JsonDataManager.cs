using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class JsonDataManager
{
    private static string savePath = Application.persistentDataPath;
    public const string UserFileName = "user";
    public const string FighterFileName = "fighter";
    public const string CupFileName = "cup";
    public static void SaveData(JObject data, string fileName)
    {
        Directory.CreateDirectory(savePath);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(getFilePath(fileName), json);
    }

    public static void ReadUserFile()
    {
        JObject userData = JsonDataManager.ReadData(JsonDataManager.UserFileName);
        UserFactory.CreateUserInstance((bool)userData["firstTime"], (string)userData["flag"], (string)userData["userIcon"], (int)userData["energy"], (int)userData["wins"], (int)userData["loses"], (int)userData["elo"], (int)userData["peakElo"], (int)userData["gold"], (int)userData["gems"], (int)userData["cups"]);
    }
    public static Fighter ReadFighterFile()
    {
        JObject fighterData = JsonDataManager.ReadData(JsonDataManager.FighterFileName);
        return FighterFactory.CreatePlayerFighterInstance((string)fighterData["fighterName"], (string)fighterData["skin"], (string)fighterData["species"],
            (float)fighterData["hp"], (float)fighterData["damage"], (float)fighterData["speed"], fighterData["skills"].ToObject<List<Skill>>(),
            (int)fighterData["level"], (int)fighterData["experiencePoints"]);
    }
    public static void ReadCupFile()
    {
        JObject cupData = JsonDataManager.ReadData(JsonDataManager.CupFileName);
        CupFactory.CreateCupInstance((string)cupData["cupName"],(bool)cupData["isActive"], (bool)cupData["playerStatus"], (string)cupData["round"], cupData["participants"].ToObject<List<CupFighter>>(), cupData["cupInfo"].ToObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>());
    }

    public static JObject ReadData(string fileName)
    {
        using (StreamReader r = new StreamReader(getFilePath(fileName)))
        {
            string json = r.ReadToEnd();
            return JObject.Parse(json);
        }
    }

    public static string getFilePath(string fileName)
    {
        return $"{savePath}\\{fileName}.txt";
    }

    //We need to create a fighter class that is not monobehaviour to be able to serialize and save the data into the JSON file.
    public static SerializableFighter CreateSerializableFighterInstance(Fighter fighter)
    {

        return new SerializableFighter(fighter);
    }
}
