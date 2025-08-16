using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.Search;
public class AskName : MonoBehaviour
{
    public string PlayerName;
    public TMP_InputField PlayerInputField;
    public Button ConfirmButton;
    public GameObject AskNamePanel;
    public GameObject LeaderBoard;
    public GameObject Achivement;
    public GameObject Profile;
    public Button ExitButton;
    public void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            AskNamePanel.SetActive(true);
            ConfirmButton.onClick.AddListener(SaveName);
        }
        else
        {
            AskNamePanel.SetActive(false);

        }
        ExitButton.gameObject.SetActive(true);
    }
    public void SaveName()
    {
        PlayerName = PlayerInputField.text;
        if (!string.IsNullOrEmpty(PlayerName))
        {
            PlayerPrefs.SetString("PlayerName", PlayerName);
            PlayerPrefs.Save();
            Debug.Log("PlayerName : " + PlayerName);
            AskNamePanel.SetActive(false);
        }
    }
    public void LeadrBoardButton()
    {
        LeaderBoard.SetActive(true);
        ExitButton.gameObject.SetActive(false);
    }
    public void AchivementButton()
    {
        Achivement.SetActive(true);
        AchievementManager.Instance.RefreshAchievementUI();
        ExitButton.gameObject.SetActive(false);
    }
    public void ProfileButton()
    {
        Profile.SetActive(true);
       
        if (PlayerProfileManager.Instance != null)
        {
            PlayerProfileManager.Instance.RefreshPlayerStats();
        }
        ExitButton.gameObject.SetActive(false);
    }
    public void BackButton()
    {
        LeaderBoard.SetActive(false);
        Achivement.SetActive(false);
        Profile.SetActive(false);
        ExitButton.gameObject.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
