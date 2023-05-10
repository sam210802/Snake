using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelSaveLoadManager
{
    static string saveLocation = Application.persistentDataPath + "/Data/Levels";
    static string defaultLevelSaveLocation ="DefaultLevels";

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
        foreach (FileInfo file in info.GetFiles()) {
            if (file.Extension != ".json") continue;

            boards.Add(load(file.Name));
        }
        return boards;
    }

    public static List<Board> loadAllDefault() {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>(defaultLevelSaveLocation);
        Debug.Log("Length: " + levelFiles[0].name);
        DirectoryInfo info = new DirectoryInfo(defaultLevelSaveLocation);
        List<Board> boards = new List<Board>();
        foreach (TextAsset file in levelFiles) {
            // gets first instance of a possitive number from file name
            int levelNumber = int.Parse(Regex.Match(file.name, "\\d+").ToString());
            boards.Add(loadDefault(levelNumber));
        }
        return boards;
    }

    public static Board load(string fileName, bool editMode = false) {
        string fileLocation = Path.Join(saveLocation, fileName);
        if (!File.Exists(fileLocation)) return null;
        return new Board(JsonUtility.FromJson<LevelData>(File.ReadAllText(fileLocation)), editMode);
    }

    public static Board loadDefault(int levelNumber) {
        string fileLocation = Path.Join(defaultLevelSaveLocation, "level_" + levelNumber.ToString("00"));
        TextAsset levelFile = Resources.Load<TextAsset>(fileLocation);
        if (levelFile == null) return null;
        return new Board(JsonUtility.FromJson<LevelData>(levelFile.ToString()));
    }

    public static string getSaveLocation()
    {
        return saveLocation;
    }
}
