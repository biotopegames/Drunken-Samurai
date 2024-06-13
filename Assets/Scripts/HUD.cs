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
    // public Animator anim;
    //[SerializeField] private GameObject healthBar;
    //[SerializeField] private GameObject startUp;
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private GameObject pauseScreen;
    private bool isPaused = false;

    [SerializeField] private GameObject levelUpScreen;
    public TextMeshProUGUI lvlText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider expSlider;

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
        //Set all bar widths to 1, and also the smooth variables.
        //healthBarWidth = 1;
        //healthBarWidthEased = healthBarWidth;

    }

    void Update()
    {
        healthSlider.maxValue = Player.Instance.stats.maxHealth;
        healthSlider.value = Player.Instance.stats.currentHealth; 
        expSlider.value = Player.Instance.stats.currentExp; 
        expSlider.maxValue = Player.Instance.stats.expNeededToLevelUp;
        lvlText.text = Player.Instance.stats.level.ToString();
        //Controls the width of the health bar based on the player's total health
        //healthBarWidth = (float)Player.Instance.health / (float)Player.Instance.maxHealth;
        // healthBarWidthEased += (healthBarWidth - healthBarWidthEased) * Time.deltaTime * healthBarWidthEased;
        // healthBar.transform.localScale = new Vector2(healthBarWidthEased, 1);
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
        // Player.Instance.stats.damage = (int)(Player.Instance.stats.damage * 1.1f); // Increase damage by 10%
        // int increaseAmount = Mathf.RoundToInt(Player.Instance.stats.damage * 0.1f);
        // Player.Instance.stats.damage += increaseAmount;

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

    public void ShowLevelUpScreen(bool showLevelUpScreen)
    {
        levelUpScreen.SetActive(showLevelUpScreen);

        if(showLevelUpScreen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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
