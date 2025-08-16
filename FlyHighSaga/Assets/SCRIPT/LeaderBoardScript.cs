using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardScript : MonoBehaviour
{
    [System.Serializable]
    public class LeaderBoardEntry
    {
        public string PlayerName;
        public int Score;

        public LeaderBoardEntry(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
    public GameObject[] ScoreSlot; 
    private List<LeaderBoardEntry> leaderboard = new List<LeaderBoardEntry>();
    private readonly string[] fakePlayerNames = {
        "Alex", "Jordan", "Casey", "Taylor", "Riley", "Avery", "Morgan", "Quinn",
        "Sage", "Blake", "Cameron", "Reese", "Skyler", "Phoenix", "River"
    };

    private void OnEnable()
    {
        LoadLeaderBoard();

        
        if (leaderboard.Count == 0)
        {
            InitializeFakeLeaderBoard();
        }

        string currentName = PlayerPrefs.GetString("PlayerName", "Unknown");
        int currentScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > 0)
        {
            UpdatePlayerScore(currentName, currentScore);
            SaveLeaderBoard();
        }
        UpdateLeaderBoardUI();
    }

    private void InitializeFakeLeaderBoard()
    {
        
        List<string> usedNames = new List<string>();

        for (int i = 0; i < 4; i++)
        {
            string fakeName;
            do
            {
                fakeName = fakePlayerNames[Random.Range(0, fakePlayerNames.Length)];
            } while (usedNames.Contains(fakeName));

            usedNames.Add(fakeName);
            int fakeScore = Random.Range(15, 95);
            leaderboard.Add(new LeaderBoardEntry(fakeName, fakeScore));
        }

        leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));
    }

    public void LoadLeaderBoard()
    {
        leaderboard.Clear();
        for (int i = 0; i < 5; i++)
        {
            string nameKey = $"LB_Name_{i}";
            string scoreKey = $"LB_Score_{i}";

            if (PlayerPrefs.HasKey(nameKey) && PlayerPrefs.HasKey(scoreKey))
            {
                string name = PlayerPrefs.GetString(nameKey);
                int score = PlayerPrefs.GetInt(scoreKey);
                leaderboard.Add(new LeaderBoardEntry(name, score));
            }
        }

        leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));
    }

    public void SaveLeaderBoard()
    {
        
        for (int i = 0; i < 10; i++) 
        {
            PlayerPrefs.DeleteKey($"LB_Name_{i}");
            PlayerPrefs.DeleteKey($"LB_Score_{i}");
        }

        for (int i = 0; i < leaderboard.Count && i < 5; i++)
        {
            PlayerPrefs.SetString($"LB_Name_{i}", leaderboard[i].PlayerName);
            PlayerPrefs.SetInt($"LB_Score_{i}", leaderboard[i].Score);
        }

        PlayerPrefs.Save();
    }

    public void UpdatePlayerScore(string playerName, int score)
    {
        if (score <= 0) return;
        LeaderBoardEntry existingEntry = null;
        foreach (var entry in leaderboard)
        {
            if (entry.PlayerName.Equals(playerName, System.StringComparison.OrdinalIgnoreCase))
            {
                existingEntry = entry;
                break;
            }
        }

        if (existingEntry != null)
        {
            if (score > existingEntry.Score)
            {
                existingEntry.Score = score;
            }
        }
        else
        {
            leaderboard.Add(new LeaderBoardEntry(playerName, score));
        }

        leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));

        if (leaderboard.Count > 5)
        {
            leaderboard.RemoveRange(5, leaderboard.Count - 5);
        }
    }

    public void UpdateLeaderBoardUI()
    {
        for (int i = 0; i < ScoreSlot.Length; i++)
        {
            TextMeshProUGUI[] texts = ScoreSlot[i].GetComponentsInChildren<TextMeshProUGUI>();

            if (i < leaderboard.Count)
            {
                foreach (var text in texts)
                {
                    if (text.name.Contains("Rank"))
                        text.text = (i + 1).ToString();
                    else if (text.name.Contains("Name"))
                        text.text = leaderboard[i].PlayerName;
                    else if (text.name.Contains("Score"))
                        text.text = leaderboard[i].Score.ToString();
                }
                string currentPlayer = PlayerPrefs.GetString("PlayerName", "Unknown");
                if (leaderboard[i].PlayerName.Equals(currentPlayer, System.StringComparison.OrdinalIgnoreCase))
                {
                    // You can add highlighting here if needed
                    // For example: ScoreSlot[i].GetComponent<Image>().color = Color.yellow;
                }
            }
            else
            {
                foreach (var text in texts)
                {
                    if (text.name.Contains("Rank"))
                        text.text = (i + 1).ToString();
                    else
                        text.text = "-";
                }
            }
        }
    }

    
    [ContextMenu("Reset Leaderboard")]
    public void ResetLeaderBoard()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey($"LB_Name_{i}");
            PlayerPrefs.DeleteKey($"LB_Score_{i}");
        }
        PlayerPrefs.Save();
        leaderboard.Clear();
        InitializeFakeLeaderBoard();
        SaveLeaderBoard();
        UpdateLeaderBoardUI();
    }
}
