using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerStats stats;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int health = 100;
    [SerializeField] private int defense = 0;

    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashCooldown = 2f; // Seconds
    [SerializeField] private float rollCooldown = 2f; // Seconds
    [SerializeField] private float attackCoolDown = 1f; // Seconds


    public float enemySpawnRadius = 10f;
    public float itemPickUpRadius = 1f;
    private RecoveryCounter recoveryCounter;

    // PS4 controls
    public float horizontal;
    public float vertical;
    public bool isRolling;
    public float rollForce;

    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private UnityEngine.Vector2 movement;
    private float currentCooldown;
    private UnityEngine.Vector2 targetPosition;
    // private bool isDashing = false;
    private CapsuleCollider2D col;
    private static Player instance;
    [SerializeField] PlayerPosition playerPos;
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
        if(playerPos != null)
        {
            transform.position = new Vector2 (playerPos.x, playerPos.y);
        }

        //ResetStats();
        moveSpeed = stats.moveSpeed;
        health = stats.currentHealth;
        dashCooldown = stats.dashCooldown;
        dashSpeed = stats.dashSpeed;
        defense = stats.defense;

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
    }

    void FixedUpdate()
    {
        if(!isRolling)
        {
            // rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        // if (isDashing)
        // {
        //     DashTowardsTarget();
        // }
    }


    public void HandleInput()
    {
        // if (Input.GetMouseButtonDown(1))
        // {
        //     targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     //isDashing = true;
        //     currentCooldown = dashCooldown; // Reset cooldown
        //     HandleAttackDirection();
        //     anim.SetTrigger("Attack"); // Play attack animation
        // }



        if (Input.GetKeyDown(KeyCode.E) && currentCooldown <= 0)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // isDashing = true;
            currentCooldown = dashCooldown; // Reset cooldown
            HandleAttackDirection();
            anim.SetTrigger("Attack"); // Play attack animation
        }

        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     LevelUp();
        // }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HUD.Instance.PauseGame();
        }
    }


    void DashTowardsTarget()
    {
        col.isTrigger = true;
        // Move towards the target position
        UnityEngine.Vector2 currentPosition = rb.position;
        UnityEngine.Vector2 direction = (targetPosition - currentPosition).normalized;
        UnityEngine.Vector2 dashPosition = currentPosition + direction * dashSpeed * Time.fixedDeltaTime;

        rb.MovePosition(dashPosition);

        // Check if the player has reached the target position (or is very close to it)
        if (UnityEngine.Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            col.isTrigger = false;
            // isDashing = false; // Stop dashing
        }
    }
    void HandleAttackDirection()
    {
        UnityEngine.Vector2 direction = new UnityEngine.Vector2(horizontal, vertical);
        direction.Normalize();
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

        public void GainHonor(int honorAmount)
    {
        stats.honor += honorAmount;
    }

    public void FreezePlayer()
    {
        rb.velocity = Vector2.zero;
        movement.x = 0;
        movement.y = 0;
        horizontal = 0;
        vertical = 0;
    }

    public void WearEquipment(EquipmentItem itemToWear, bool equip)
    {
                if(equip)
        {
            stats.maxHealth += itemToWear.healthStat;
            stats.damage += itemToWear.damageStat;
            stats.defense += itemToWear.defenseStat;
            stats.moveSpeed += itemToWear.moveSpeedStat;
        }
        else
        {
            stats.maxHealth -= itemToWear.healthStat;
            stats.damage -= itemToWear.damageStat;
            stats.defense -= itemToWear.defenseStat;
            stats.moveSpeed -= itemToWear.moveSpeedStat;
        }
    }
    public void LevelUp()
    {
        stats.level++;
        stats.currentExp = 0;
        HUD.Instance.ShowLevelUpScreen(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.levelUpSound, 0.05f);
    }

    public void LastFacingDirection()
    {
        // if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
        // {
        //     anim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
        //     anim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        // }

        if (Mathf.Round(Mathf.Abs(horizontal)) == 1 || Mathf.Round(Mathf.Abs(vertical)) == 1)
        {
            //Debug.Log(Mathf.Round(Mathf.Abs(horizontal)));
            anim.SetFloat("LastMoveX", horizontal);
            anim.SetFloat("LastMoveY", vertical);
            anim.SetFloat("TargetPosX", horizontal);
            anim.SetFloat("TargetPosY", vertical);
        }
    }

        public void ShowInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // Perform actions when the move action starts
            return;

        }
        // Check if the action phase is Cancelled (OnCancelled event)
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Perform actions when the move action is cancelled
            return;

        }
        Debug.Log("opened inventory");

        HUD.Instance.OpenInventory();
    }


    public void EquipItem(InputAction.CallbackContext context)
    {
        //if the selected itemslot is medallion and there is an item there unequip and put it in the first available itemSlot

        if (context.phase == InputActionPhase.Started)
        {
            return;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            return;
        }
        Inventory.Instance.EquipSelectedItem();
    }

        public void GrabItem (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            return;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            return;
        }
        Inventory.Instance.GrabAndPlaceItem();
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<UnityEngine.Vector2>().x;
        vertical = context.ReadValue<UnityEngine.Vector2>().y;
        movement.x = horizontal;
        movement.y = vertical;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        LastFacingDirection();
    }

public void Roll(InputAction.CallbackContext context)
{
    if (currentCooldown <= 0 && !isRolling) 
    {
        isRolling = true;
        anim.SetBool("isRolling", true);
        currentCooldown = dashCooldown; // Reset cooldown
        anim.SetTrigger("Roll"); // Play attack animation
        UnityEngine.Vector2 rollDirection = new UnityEngine.Vector2(anim.GetFloat("LastMoveX"), anim.GetFloat("LastMoveY")).normalized;
        //Debug.Log("Roll Direction: " + rollDirection);
            horizontal = 0;
    vertical = 0;
    rb.velocity = Vector2.zero;
        rb.AddForce(rollDirection * rollForce, ForceMode2D.Impulse);
        //Debug.Log("Force Applied: " + rollDirection);
        StartCoroutine(StopRolling());
    }
}

public IEnumerator StopRolling()
{
    yield return new WaitForSeconds(0.3f);
    anim.SetBool("isRolling", false);
    //movement = Vector2.zero;
    isRolling = false;
}

    public void Attack(InputAction.CallbackContext context)
    {
        if (currentCooldown <= 0)
        {
        //LastFacingDirection();

            //targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentCooldown = dashCooldown; // Reset cooldown
            //HandleAttackDirection();
            anim.SetTrigger("Attack"); // Play attack animation
            moveSpeed = 0;
            
            StartCoroutine(EnableMovementAfterDelay(0.5f)); // Adjust the delay as needed
        }
    }

    public void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = stats.moveSpeed;
        }
    }
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        // Enable movement after the delay
        moveSpeed = stats.moveSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemySpawnRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, itemPickUpRadius);

    }
}