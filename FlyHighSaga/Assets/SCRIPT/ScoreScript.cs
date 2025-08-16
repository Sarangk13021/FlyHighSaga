using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScore;
    public AudioClip Point;
    public FlyScript flyscript;
    private int score = 0;
    void Start()
    {
        score = 0;
        ScoreText.text = score.ToString();
        HighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreTrig"))
        {
            flyscript.AudioSrc.PlayOneShot(Point);
            score++;
            ScoreText.text = score.ToString("0");
            //Check achievements when score increases
            if (AchievementManager.Instance != null)
            {
                AchievementManager.Instance.CheckAchievements(score);
            }
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
                HighScore.text = score.ToString();
            }
        }

    }
    //Call this when player dies
    public void OnPlayerDeath()
    {
        // Increment death counter
        if (PlayerProfileManager.Instance != null)
        {
            PlayerProfileManager.Instance.IncrementDeathCount();
        }

        Debug.Log("Player died - Death count updated");
    }
}
