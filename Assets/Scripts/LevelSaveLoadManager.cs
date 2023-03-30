using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelSaveLoadManager
{
    static string saveLocation = Application.persistentDataPath + "/Data/Levels";
    static string defaultLevelSaveLocation = "Assets/Data/Levels";

    public static void save(Board board) {
        Directory.CreateDirectory(saveLocation);
        string fileLocation = saveLocation + "/" + board.levelNameProperty + ".json";
        if (!File.Exists(fileLocation)) {
            string dataString = JsonUtility.ToJson(board.toData());
            File.WriteAllText(fileLocation, dataString);
        }
    }

    public static List<Board> loadAll() {
        DirectoryInfo info = new DirectoryInfo(saveLocation);
        List<Board> boards = new List<Board>();
        foreach (FileInfo file in info.GetFiles("*.json")) {
            string[] fileSplit = file.ToString().Split("\\");
            string fileName = fileSplit[fileSplit.Length - 1];
            boards.Add(load(fileName));
        }
        return boards;
    }

    public static List<Board> loadAllDefault() {
        DirectoryInfo info = new DirectoryInfo(defaultLevelSaveLocation);
        List<Board> boards = new List<Board>();
        foreach (FileInfo file in info.GetFiles("*.json")) {
            string[] fileSplit = file.ToString().Split("\\");
            // gets first instance of a possitive number from string
            int levelNumber = int.Parse(Regex.Match(fileSplit[fileSplit.Length - 1], "\\d+").ToString());
            boards.Add(loadDefault(levelNumber));
        }
        return boards;
    }

    public static Board load(string fileName, bool editMode = false) {
        string fileLocation = saveLocation + "/" + fileName;
        if (!File.Exists(fileLocation)) return null;
        return new Board(JsonUtility.FromJson<LevelData>(File.ReadAllText(fileLocation)), editMode);
    }

    public static Board loadDefault(int levelNumber) {
        string fileLocation = defaultLevelSaveLocation + "/" + "level_" + levelNumber.ToString("00") + ".json";
        if (!File.Exists(fileLocation)) return null;
        return new Board(JsonUtility.FromJson<LevelData>(File.ReadAllText(fileLocation)));
    }

    public static string getSaveLocation()
    {
        return saveLocation;
    }
}
