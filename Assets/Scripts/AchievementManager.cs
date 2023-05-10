using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    public static AchievementManager instance;
    private static List<Achievement> achievements;
    private IAchievement achievementSystem;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
        // if on platform with achievements this will return correct API
        // e.g.. PlayStore API, Steam API, XBOX API
        achievementSystem = GetComponent<IAchievement>();
        Debug.Log(achievementSystem);
        InitializeAchievements();
    }

    void Update() {
        CheckAchievementCompletion();
    }

    public void InitializeAchievements() {
        if (achievements != null) return;

        achievements = new List<Achievement>();

        string achievememtTitle;

        // food achievements
        achievememtTitle = "Apple_Eater";
        achievements.Add(new Achievement(achievememtTitle, "Eat 10 Apples", (object o) => PlayerPrefs.GetInt("Apples_Eaten", 0) >= 10 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Apple_Destroyer";
        achievements.Add(new Achievement(achievememtTitle, "Eat 50 Apples", (object o) => PlayerPrefs.GetInt("Apples_Eaten", 0) >= 50 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Apple_Anialator";
        achievements.Add(new Achievement(achievememtTitle, "Eat 100 Apples", (object o) => PlayerPrefs.GetInt("Apples_Eaten", 0) >= 100 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));

        // death achievements
        achievememtTitle = "Snake_Novice";
        achievements.Add(new Achievement(achievememtTitle, "Die 10 Times", (object o) => PlayerPrefs.GetInt("Total_Deaths", 0) >= 10 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Snake_Rookie";
        achievements.Add(new Achievement(achievememtTitle, "Die 50 Times", (object o) => PlayerPrefs.GetInt("Total_Deaths", 0) >= 50 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Snake_Noob";
        achievements.Add(new Achievement(achievememtTitle, "Die 100 Times", (object o) => PlayerPrefs.GetInt("Total_Deaths", 0) >= 100 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));

        // level completion achievements
        achievememtTitle = "Level_Completer";
        achievements.Add(new Achievement(achievememtTitle, "Complete 5 Levels", (object o) => PlayerPrefs.GetInt("Levels_Fully_Completed", 0) >= 5 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Level_Expert";
        achievements.Add(new Achievement(achievememtTitle, "Complete 10 Levels", (object o) => PlayerPrefs.GetInt("Levels_Fully_Completed", 0) >= 10 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Level_God";
        achievements.Add(new Achievement(achievememtTitle, "Complete 20 Levels", (object o) => PlayerPrefs.GetInt("Levels_Fully_Completed", 0) >= 20 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));

        // achievement completion achievements
        achievememtTitle = "Achievement_Completer";
        achievements.Add(new Achievement(achievememtTitle, $"Achieve 5 Achievements",
            (object o) => PlayerPrefs.GetInt("Achievements_Achieved", 0) >= 5 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Achievement_Expert";
        achievements.Add(new Achievement(achievememtTitle, $"Achieve 10 Achievements",
            (object o) => PlayerPrefs.GetInt("Achievements_Achieved", 0) >= 10 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        achievememtTitle = "Achievement_God";
        achievements.Add(new Achievement(achievememtTitle, $"Achieve 12 Achievements",
            (object o) => PlayerPrefs.GetInt("Achievements_Achieved", 0) >= 12 &&
            Convert.ToBoolean(PlayerPrefs.GetInt($"{achievememtTitle}_Completed", Convert.ToInt32(false))) == false));
        CheckAchievementCompletion();
    }

    // checks each achievememt to see if they've been achieved
    public void CheckAchievementCompletion() {
        if (achievements == null) return;

        foreach (Achievement achievement in achievements) {
            achievement.UpdateCompletion();
            // if another achievement system update achievement state for that achievement system
            if (achievementSystem != null) {
                achievementSystem.UpdateAchievement(achievement, achievement.achieved);
            }
        }
    }

    // returns true if achievement has been achieved else returns false
    public static bool AchievementUnlocked(string achievementTitle) {
        return Convert.ToBoolean(PlayerPrefs.GetInt($"{achievementTitle}_Completed", Convert.ToInt32(false)));
    }

    public List<Achievement> getAchievements() {
        return achievements;
    }
}

public class Achievement {
    public string title;
    public string description;
    public Predicate<object> requirement;
    public bool achieved;

    public Achievement(string tile, string description, Predicate<object> requirement) {
        this.title = tile;
        this.description = description;
        this.requirement = requirement;
    }

    public void UpdateCompletion() {
        if (Convert.ToBoolean(PlayerPrefs.GetInt($"{title}_Completed", Convert.ToInt32(false)))) {
            achieved = true;
            return;
        }

        if (RequirementsMet()) {
            Debug.Log($"{title}: {description}");
            // increments achievements completed to one
            PlayerPrefs.SetInt("Achievements_Achieved", PlayerPrefs.GetInt("Achievements_Achieved", 0) + 1);
            // sets achievement state to completed so achievement can't be complted again
            PlayerPrefs.SetInt($"{title}_Completed", Convert.ToInt32(true));
            achieved = true;
        }
    }

    public bool RequirementsMet() {
        return requirement.Invoke(null);
    }
}
