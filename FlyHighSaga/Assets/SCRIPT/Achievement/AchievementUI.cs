using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button collectButton;
    public GameObject lockOverlay; 

    [Header("Achievement Sprite")]
    //public Sprite achievementSprite; 

    [Header("Visual Settings")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    private Achievement currentAchievement;
    private Image achievementIcon; 

    public AudioSource Audio;
    public AudioClip CollectAchi;

    void Start()
    {
        
        achievementIcon = GetComponentInChildren<Image>();
        if (achievementIcon == null)
        {
            Debug.LogError("No Image component found in " + gameObject.name + " or its children!");
        }
    }

    public void SetupAchievement(Achievement achievement)
    {
        currentAchievement = achievement;

        titleText.text = achievement.title;
        descriptionText.text = achievement.description;

        if (achievement.isCollected)
        {
            
            collectButton.gameObject.SetActive(false);
            if (lockOverlay != null)
                lockOverlay.SetActive(false);
            SetUnlockedAppearance();
        }
        else if (achievement.isUnlocked)
        {
            
            collectButton.gameObject.SetActive(true);
            collectButton.interactable = true;
            collectButton.GetComponentInChildren<TextMeshProUGUI>().text = "COLLECT";
            if (lockOverlay != null)
                lockOverlay.SetActive(false);
            SetUnlockedAppearance();
        }
        else
        {
           
            collectButton.gameObject.SetActive(false);
            if (lockOverlay != null)
                lockOverlay.SetActive(true);
            SetLockedAppearance();
        }
    }

    void SetLockedAppearance()
    {
        if (achievementIcon != null)
            achievementIcon.color = lockedColor;
        titleText.color = lockedColor;
        descriptionText.color = lockedColor;
    }

    void SetUnlockedAppearance()
    {
        if (achievementIcon != null)
            achievementIcon.color = unlockedColor;
        titleText.color = unlockedColor;
        descriptionText.color = unlockedColor;
    }

    public void OnCollectButtonClicked()
    {
        if (currentAchievement != null && currentAchievement.isUnlocked && !currentAchievement.isCollected)
        {
            Audio.PlayOneShot(CollectAchi);
            Debug.Log($"Collecting achievement: {currentAchievement.title}");

            AchievementManager.Instance.CollectAchievement(currentAchievement.achievementID);

            
            collectButton.gameObject.SetActive(false);

            // Update visual state
            SetUnlockedAppearance();
        }
    }
}