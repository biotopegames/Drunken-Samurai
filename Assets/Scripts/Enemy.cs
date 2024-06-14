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
        [SerializeField] private bool hasFought = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //stats = GetComponent<Stats>();
        //speed = stats.moveSpeed;
        //rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        // Find the player GameObject by tag and store its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && hasFought == false)
        {
            BattleHUD.Instance.StartBattle();
        }
    }

}
