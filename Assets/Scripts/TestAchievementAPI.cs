using System.Collections.Generic;
using UnityEngine;

public class TestAchievementAPI : MonoBehaviour, IAchievement {

    public static TestAchievementAPI instance;
    List<Achievement> completedAchievements;

    void Awake() {
        // API initialisation
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        completedAchievements = new List<Achievement>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateAchievement(Achievement achievement, bool completed) {
        // Code to unlock achievement for current platform
        if (completed && !completedAchievements.Contains(achievement)) {
            Debug.Log($"{achievement.title} completed");
            completedAchievements.Add(achievement);
        }
    }
}