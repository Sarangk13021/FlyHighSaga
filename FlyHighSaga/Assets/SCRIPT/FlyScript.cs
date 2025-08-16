using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FlyScript : MonoBehaviour
{
    [SerializeField] private float force = 4.5f;
    [SerializeField] private float rotation_Speed = 5f;
    private Rigidbody2D rg;

    public GameObject GameOver;
    
    public AudioClip Jump;
    public AudioClip Dead;
    public AudioSource AudioSrc;

    
    private static int lossCounter = 0;
    [SerializeField] private int adFrequency = 5;
    void Start()
    {
        GameOver.SetActive(false);
        AudioSrc = GetComponent<AudioSource>();
        rg = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rg.velocity.y * rotation_Speed);
    }
    void Update()
    {
     
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
        {
            Fly();
        }
    }
    public void Fly()
    {
        rg.velocity = Vector2.zero;
        rg.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        AudioSrc.PlayOneShot(Jump);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pipe") || (collision.collider.CompareTag("Ground")))
        {
            AudioSrc.PlayOneShot(Dead);
            lossCounter++;
            Debug.Log($"Player died. Loss count: {lossCounter}");

            if (lossCounter >= adFrequency)
            {
                ShowInterstitialAd();
                lossCounter = 0;
            }
            GameOver.SetActive(true);

            //DIRECT DEATH TRACKING 
            int currentDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            currentDeaths++;
            PlayerPrefs.SetInt("TotalDeaths", currentDeaths);
            PlayerPrefs.Save();
            Debug.Log($"Death count saved directly: {currentDeaths}");

            Time.timeScale = 0;
        }
    }
    private void ShowInterstitialAd()
    {
        
        if (AdsManager.Instance != null && AdsManager.Instance.interstatialads != null)
        {
            Debug.Log("Showing interstitial ad after 5 losses");
            AdsManager.Instance.interstatialads.ShowInterstatialAd();
        }
        else
        {
            Debug.LogWarning("AdsManager or InterstatialAds not available");
        }
    }
    public void PlayButton()
    {
        GameOver.SetActive(false);
        RestartGame();
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void HomeScreenButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
