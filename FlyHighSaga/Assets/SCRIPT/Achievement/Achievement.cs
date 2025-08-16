using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string achievementID;
    public string title;
    public string description;
    public int requiredScore;
    public bool isUnlocked;
    public bool isCollected;

    public Achievement(string id, string title, string desc, int score)
    {
        achievementID = id;
        this.title = title;
        description = desc;
        requiredScore = score;
        isUnlocked = false;
        isCollected = false;
    }
}