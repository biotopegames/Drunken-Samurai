using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*Manages and updates the HUD, which contains your health bar, coins, etc*/

public class HUD : MonoBehaviour
{
    [Header("Reference")]
    public Animator anim;
    //[SerializeField] private GameObject healthBar;
    //[SerializeField] private GameObject startUp;
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private GameObject pauseScreen;
    private bool isPaused = false;


    private float healthBarWidth;
    private float healthBarWidthEased;
    [System.NonSerialized] public string loadSceneName;
    [System.NonSerialized] public bool resetPlayer;

    private static HUD instance;
    public static HUD Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<HUD>();
            return instance;
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        deadScreen.SetActive(false);


    }

    void Update()
    {

    }

    public void HealthBarHurt()
    {
        //animator.SetTrigger("hurt");
    }

    public void LostGame()
    {
        deadScreen.SetActive(true);
        Time.timeScale = 0;
    }

    



    public void IncreaseDamage()
    {
        Player.Instance.stats.damage ++;
    }

    public void IncreaseMoveSpeed()
    {
        Player.Instance.stats.moveSpeed *= 1.2f; // Increase moveSpeed by 10%
    }

    public void IncreaseHealth()
    {
    int increaseAmount = Mathf.RoundToInt(Player.Instance.stats.maxHealth * 0.1f);
    Player.Instance.stats.maxHealth += increaseAmount;
    Player.Instance.stats.currentHealth = Player.Instance.stats.maxHealth;
    }

    public void DecreaseDashCooldown()
    {
        Player.Instance.stats.dashCooldown *= 0.8f; // Decrease dashCooldown by 10%
    }

    public void PauseGame()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.gemSound, 0.15f, 0.1f);
        if(!isPaused)
        {
        pauseScreen.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
        }
        else
        {
        pauseScreen.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        }
    }
}
