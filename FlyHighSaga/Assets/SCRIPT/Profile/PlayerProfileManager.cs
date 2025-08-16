using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerProfileManager : MonoBehaviour
{
    [Header("Player Info UI")]
    public TextMeshProUGUI playerNameDisplay; 
    public TextMeshProUGUI totalDeathsText;
    public TextMeshProUGUI bestScoreText;

    [Header("Achievement Slots (6 Empty Slots)")]
    public Image[] achievementSlots = new Image[6]; 

    [Header("Achievement Sprites")]
    public Sprite bronzeSprite;
    public Sprite silverSprite;
    public Sprite goldSprite;
    public Sprite platinumSprite;
    public Sprite masterSprite;
    public Sprite grandMasterSprite;
    public Sprite emptySlotSprite; 

    [Header("Visual Settings")]
    public Color emptySlotColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    public Color collectedSlotColor = Color.white;

    public static PlayerProfileManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DON'T use DontDestroyOnLoad for UI-heavy managers
            Debug.Log("PlayerProfileManager Instance created");
        }
        else if (Instance != this)
        {
            Debug.Log("Duplicate PlayerProfileManager destroyed");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        LoadPlayerProfile();
        RefreshAchievementSlots();
    }

    void LoadPlayerProfile()
    {
        
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        if (playerNameDisplay != null)
            playerNameDisplay.text = playerName;

        // Load death count
        int totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        if (totalDeathsText != null)
            totalDeathsText.text = totalDeaths.ToString();

        // Load best score
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        Debug.Log($"Loaded Profile - Name: {playerName}, Deaths: {totalDeaths}, Best Score: {bestScore}");
    }

    public void IncrementDeathCount()
    {
        int currentDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        currentDeaths++;
        PlayerPrefs.SetInt("TotalDeaths", currentDeaths);
        PlayerPrefs.Save();

        if (totalDeathsText != null)
            totalDeathsText.text = currentDeaths.ToString();

        Debug.Log($"Death count increased to: {currentDeaths}");
    }

    public void RefreshAchievementSlots()
    {
        if (AchievementManager.Instance == null)
        {
            Debug.LogWarning("AchievementManager not found!");
            return;
        }

        var achievements = AchievementManager.Instance.achievements;

        Dictionary<string, Sprite> achievementSprites = new Dictionary<string, Sprite>
        {
            {"bronze", bronzeSprite},
            {"silver", silverSprite},
            {"gold", goldSprite},
            {"platinum", platinumSprite},
            {"master", masterSprite},
            {"grandmaster", grandMasterSprite}
        };

        for (int i = 0; i < achievementSlots.Length && i < achievements.Count; i++)
        {
            if (achievementSlots[i] == null) continue;

            var achievement = achievements[i];

            if (achievement.isCollected)
            {
                if (achievementSprites.ContainsKey(achievement.achievementID))
                {
                    achievementSlots[i].sprite = achievementSprites[achievement.achievementID];
                    achievementSlots[i].color = collectedSlotColor;
                }
                Debug.Log($"Slot {i}: Showing collected {achievement.title}");
            }
            else
            {
                achievementSlots[i].sprite = emptySlotSprite;
                achievementSlots[i].color = emptySlotColor;
                Debug.Log($"Slot {i}: Empty (achievement not collected)");
            }
        }
    }

    public void RefreshPlayerStats()
    {
        LoadPlayerProfile();
        RefreshAchievementSlots();
    }
}