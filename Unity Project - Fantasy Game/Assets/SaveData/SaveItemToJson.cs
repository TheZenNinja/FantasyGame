using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

public static class SaveItemToJson
{
    private static readonly string PATH = Application.dataPath + "/SaveData/";

    public static void SaveItem(Item item)
    {
        string jsonData;
        //Debug.Log(item.GetType());
        /*if (item.GetType() == typeof(ModularItem))
            jsonData = JsonUtility.ToJson(new ItemSaveData(item as ModularItem));
        else if (item.GetType() == typeof(PartItem))
            jsonData = JsonUtility.ToJson(new ItemSaveData(item as PartItem));
        else if (item.GetType() == typeof(MaterialItem))
            jsonData = JsonUtility.ToJson(new ItemSaveData(item as MaterialItem));
        else*/
        jsonData = JsonUtility.ToJson(new ItemSaveData(item));

        //Debug.Log("Saved Item at:\n" + PATH + "testBasic.json");
        //File.WriteAllText(path + item.name + ".json", jsonData);
        File.WriteAllText(PATH + "testBasic.json", jsonData);
    }

    public static Item LoadItem(string fileName)
    {
        Debug.Log("loading at:\n" + PATH + fileName + ".json");
        ItemSaveData output = JsonUtility.FromJson<ItemSaveData>(File.ReadAllText(PATH + fileName + ".json"));
        return output.ToItem();
    }

    public static void SaveData(PlayerSaveData saveData)
    {

        /*using (FileStream stream = File.Create(PATH + "newSave.sav"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);
            Debug.Log("File Saved");
        }*/
        string jsonData = JsonUtility.ToJson(saveData);
        try
        {
            File.WriteAllText(PATH + "data.json", PrettifyJSON(jsonData));
            Debug.Log("File saved");
        }
        catch (System.Exception)
        {
            throw new System.Exception("Saving didnt work");
        }
        
    }

    public static PlayerSaveData LoadData()
    {

        /*using (FileStream stream = File.Open(PATH + "newSave.sav", FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PlayerSaveData saveData = (PlayerSaveData)formatter.Deserialize(stream);
            Debug.Log("File Loaded");
            return saveData;
        }*/
        try
        {
            string data = File.ReadAllText(PATH + "data.json");

            Debug.Log("File Loaded");
            return JsonUtility.FromJson<PlayerSaveData>(SimplifyJSON(data));
        }
        catch (System.Exception)
        {
            throw new System.Exception("Loading didnt work");
        }
    }

    private static string PrettifyJSON(string input)
    {
        string output;
        output = input.Replace("},{\"itemCount", "},\n\t{\"itemCount");
        output = output.Replace("\"items\":[", "\n\n\"items\":[\n\t");
        
        return output;
    }
    private static string SimplifyJSON(string input)
    {
        string output;
        output = input.Replace("},\n\t{\"itemCount", "},{\"itemCount");
        output = output.Replace("\n\n\"items\":[\n\t", "\"items\":[");

        return output;
    }
}