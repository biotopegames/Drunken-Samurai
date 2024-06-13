using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerStats stats;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int health = 100;
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashCooldown = 2f; // Seconds
    public float enemySpawnRadius = 10f;
    public float itemPickUpRadius = 1f;
    private RecoveryCounter recoveryCounter;

    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float currentCooldown;
    private Vector2 targetPosition;
    private bool isDashing = false;
    private CapsuleCollider2D col;
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetStats();
        moveSpeed = stats.moveSpeed;
        health = stats.currentHealth;
        dashCooldown = stats.dashCooldown;
        dashSpeed = stats.dashSpeed;

        recoveryCounter = GetComponent<RecoveryCounter>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        LastFacingDirection();
        HandleInput();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        if (isDashing)
        {
            DashTowardsTarget();
        }
    }
    public void LastFacingDirection()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
        {
            anim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(1) && currentCooldown <= 0)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //isDashing = true;
            currentCooldown = dashCooldown; // Reset cooldown
            HandleAttackDirection();
            anim.SetTrigger("Attack"); // Play attack animation
        }

        if (Input.GetKeyDown(KeyCode.E) && currentCooldown <= 0)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDashing = true;
            currentCooldown = dashCooldown; // Reset cooldown
            HandleAttackDirection();
            anim.SetTrigger("Attack"); // Play attack animation
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HUD.Instance.PauseGame();
        }
    }

    void DashTowardsTarget()
    {
        col.isTrigger = true;
        // Move towards the target position
        Vector2 currentPosition = rb.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;
        Vector2 dashPosition = currentPosition + direction * dashSpeed * Time.fixedDeltaTime;

        rb.MovePosition(dashPosition);

        // Check if the player has reached the target position (or is very close to it)
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            col.isTrigger = false;
            isDashing = false; // Stop dashing
        }
    }
    void HandleAttackDirection()
    {
        // Convert mouse position into world space
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Get direction from player to the mouse click position
        Vector2 direction = clickPosition - (Vector2)transform.position;
        // Normalize the direction
        direction.Normalize();

        // Assuming your blend tree is set up to handle directions based on 
        // the X and Y values of the direction vector (from -1 to 1)
        anim.SetFloat("TargetPosX", direction.x);
        anim.SetFloat("TargetPosY", direction.y);
        anim.SetFloat("LastMoveX", direction.x);
        anim.SetFloat("LastMoveY", direction.y);
    }

    public void GetHurt(int dmgAmount)
    {
        if (!recoveryCounter.recovering)
        {
            recoveryCounter.counter = 0;
            stats.currentHealth -= dmgAmount;
            anim.SetTrigger("Hurt");
            if (stats.currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        HUD.Instance.LostGame();
        //Destroy(this.gameObject, 0.3f);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GetHurt(other.gameObject.GetComponent<Stats>().damage);
        }
    }

    public void GainExp(int xpAmount)
    {
        stats.currentExp += xpAmount;
        if (stats.currentExp >= stats.expNeededToLevelUp)
        {
            LevelUp();
            int increaseAmount = Mathf.RoundToInt(stats.expNeededToLevelUp * 1.2f);
            stats.expNeededToLevelUp = increaseAmount;
        }
    }

    public void ResetStats()
    {
        stats.maxHealth = 100;
        stats.currentHealth = stats.maxHealth;
        stats.currentExp = 0;
        stats.level = 1;
        stats.expNeededToLevelUp = 100;
        stats.moveSpeed = 0.7f;
        stats.damage = 1;
        stats.moveSpeed = 1;
        stats.dashCooldown = 0.5f;
    }

    public void GainHealth(int healthAmount)
    {
        stats.currentHealth += healthAmount;
    }

    public void LevelUp()
    {
        stats.level++;
        stats.currentExp = 0;
        HUD.Instance.ShowLevelUpScreen(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.levelUpSound, 0.05f);

        //int x;
        // if (stats.currentExp > stats.expNeededToLevelUp)
        // {
        //     x = stats.expNeededToLevelUp - stats.currentExp;
        //     GainExp(x);
        // }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemySpawnRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, itemPickUpRadius);
        
    }
}