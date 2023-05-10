using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField]
    GameObject achievementPrefab;

    [SerializeField]
    Transform completedAchievementsGridLayout;

    [SerializeField]
    Transform uncompletedAchievementsGridLayout;

    void OnEnable() {
        List<Achievement> achievements = AchievementManager.instance.getAchievements();
        resetCompletedGrid();
        resetUncompletedGrid();
        foreach (Achievement achievement in achievements) {
            if (achievement.achieved) {
                newAchievement(achievement.title, achievement.description, completedAchievementsGridLayout);
            } else {
                newAchievement(achievement.title, achievement.description, uncompletedAchievementsGridLayout);
            }
        }

        if (completedAchievementsGridLayout.childCount == 0) {
            newAchievement("None", "", completedAchievementsGridLayout);
        }

        if (uncompletedAchievementsGridLayout.childCount == 0) {
            newAchievement("None", "", uncompletedAchievementsGridLayout);
        }
    }

    void resetCompletedGrid() {
        foreach (Transform child in completedAchievementsGridLayout) {
            Destroy(child.gameObject);
        }
    }

    void resetUncompletedGrid() {
        foreach (Transform child in uncompletedAchievementsGridLayout) {
            Destroy(child.gameObject);
        }
    }

    GameObject newAchievement(string title, string description, Transform transform) {
        GameObject achievement = Instantiate(achievementPrefab, transform);
        achievement.transform.Find("Title Text (TMP)").GetComponent<TMP_Text>().text = title;
        achievement.transform.Find("Description Text (TMP)").GetComponent<TMP_Text>().text = description;
        return achievement;
    }
}
