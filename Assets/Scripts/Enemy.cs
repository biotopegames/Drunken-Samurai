using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Stats stats;
    private float speed; // Speed of the enemy
    private Rigidbody2D rb;
    private Transform playerTransform; // To store the player's position
    private Animator anim;
    private bool isAlive = true;
    [SerializeField] GameObject itemDrop;
    public float hurtSoundVolume = 0.4f;
    private RecoveryCounter recoveryCounter;
    public bool isKnockedBack;

    // Start is called before the first frame update
    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<Stats>();
        speed = stats.moveSpeed;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        // Find the player GameObject by tag and store its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // FixedUpdate is called once per frame for physics updates
    void FixedUpdate()
    {
        if (playerTransform != null && stats.health > 0 && !isKnockedBack)
        {
            // Calculate the direction vector from the enemy to the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Move the enemy towards the player
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    public void GetHurt(int dmgAmount)
    {
        if (isAlive && !recoveryCounter.recovering)
        {
            recoveryCounter.counter = 0;
            stats.health -= dmgAmount;
            anim.SetTrigger("Hurt");
            SoundManager.Instance.PlaySound(SoundManager.Instance.hurtSound, hurtSoundVolume, 0.2f);

            if (stats.health <= 0)
            {
                Die();
            }
        }
    }

    // Method to apply knockback to the enemy
    public IEnumerator KnockBack(Vector2 attackDirection, float knockBackForce)
    {
        if(!isKnockedBack)
        {
        rb.velocity = Vector2.zero;
        isKnockedBack = true;
        // Calculate the opposite direction of the attack
        Vector2 knockBackDirection = attackDirection.normalized;

        // Apply a force to the enemy in the opposite direction
        rb.AddForce(knockBackDirection * knockBackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(.5f);
        isKnockedBack = false;
        }
    }



    public void Die()
    {

        int x;
        x = Random.Range(0, 16);
        if(x == 15)
        {
            GameManager.Instance.SpawnItem();
        }

        isAlive = false; 
        GameManager.Instance.enemiesKilled++;
        GameManager.Instance.killsFromThisWave++;
        DropItem();
        GameManager.Instance.SpawnEnemy();
        //Player.Instance.GainExp(stats.expGain);
        Destroy(this.gameObject, 0.3f);
    }

    public void DropItem()
    {
        int x;
        x = Random.Range(0, 3);
        if(x != 0 && itemDrop != null)
        {
            itemDrop.GetComponent<Consumable>().expAmount = stats.expGain;
            itemDrop.GetComponent<Consumable>().honorAmount = stats.honorGain;

            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }
    }
}
