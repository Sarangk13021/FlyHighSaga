using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Achievement UI")]
    public Transform achievementParent;

    public static AchievementManager Instance;
    public List<Achievement> achievements = new List<Achievement>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
            LoadAchievements();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void InitializeAchievements()
    {
        
        if (achievements.Count > 0)
        {
            Debug.Log("Achievements already initialized, skipping...");
            return;
        }

        achievements.Add(new Achievement("bronze", "BRONZE", "Get 5 Score in game", 5));
        achievements.Add(new Achievement("silver", "SILVER", "Get 10 Score in game", 10));
        achievements.Add(new Achievement("gold", "GOLD", "Get 20 Score in game", 20));
        achievements.Add(new Achievement("platinum", "PLATINUM", "Get 50 Score in game", 50));
        achievements.Add(new Achievement("master", "MASTER", "Get 100 Score in game", 100));
        achievements.Add(new Achievement("grandmaster", "GRAND MASTER", "Get 200 Score in game", 200));

        Debug.Log("Achievements initialized: " + achievements.Count);
    }

    public void CheckAchievements(int currentScore)
    {
        foreach (var achievement in achievements)
        {
            if (!achievement.isUnlocked && currentScore >= achievement.requiredScore)
            {
                UnlockAchievement(achievement);
            }
        }
    }

    void UnlockAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;
        SaveAchievements();
        Debug.Log($"Achievement Unlocked: {achievement.title}");
    }

    
    public void CollectAchievement(string achievementID)
    {
        Achievement achievement = achievements.Find(a => a.achievementID == achievementID);
        if (achievement != null && achievement.isUnlocked && !achievement.isCollected)
        {
            achievement.isCollected = true;
            SaveAchievements();
            Debug.Log($"Achievement Collected: {achievement.title}");

            //REFRESH PLAYER PROFILE SLOTS when achievement is collected
            if (PlayerProfileManager.Instance != null)
            {
                PlayerProfileManager.Instance.RefreshAchievementSlots();
            }
        }
    }

    public void RefreshAchievementUI()
    {
        if (achievementParent == null)
        {
            FindAchievementParent();
        }

        if (achievementParent == null)
        {
            Debug.LogError("Could not find Achievement Parent!");
            return;
        }

        AchievementUI[] achievementUIs = achievementParent.GetComponentsInChildren<AchievementUI>();

        // ENSURE CORRECT COUNTS
        if (achievements.Count != 6)
        {
            Debug.LogError($"Expected 6 achievements, found {achievements.Count}! Clearing and reinitializing...");
            achievements.Clear();
            InitializeAchievements();
            LoadAchievements();
        }

        for (int i = 0; i < achievementUIs.Length && i < achievements.Count; i++)
        {
            achievementUIs[i].SetupAchievement(achievements[i]);
        }
    }

    void FindAchievementParent()
    {
        GameObject contentObj = GameObject.Find("Content");
        if (contentObj != null)
        {
            achievementParent = contentObj.transform;
            Debug.Log("Found Achievement Parent automatically: " + contentObj.name);
            return;
        }

        AchievementUI[] achievementUIs = FindObjectsOfType<AchievementUI>();
        if (achievementUIs.Length > 0)
        {
            achievementParent = achievementUIs[0].transform.parent;
            Debug.Log("Found Achievement Parent via AchievementUI: " + achievementParent.name);
        }
    }

    void SaveAchievements()
    {
        foreach (var achievement in achievements)
        {
            PlayerPrefs.SetInt($"Achievement_{achievement.achievementID}_Unlocked", achievement.isUnlocked ? 1 : 0);
            PlayerPrefs.SetInt($"Achievement_{achievement.achievementID}_Collected", achievement.isCollected ? 1 : 0);
        }
        PlayerPrefs.Save();
        Debug.Log("Achievements saved to PlayerPrefs");
    }

    void LoadAchievements()
    {
        foreach (var achievement in achievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt($"Achievement_{achievement.achievementID}_Unlocked", 0) == 1;
            achievement.isCollected = PlayerPrefs.GetInt($"Achievement_{achievement.achievementID}_Collected", 0) == 1;
        }
        Debug.Log("Achievements loaded from PlayerPrefs");
    }
}