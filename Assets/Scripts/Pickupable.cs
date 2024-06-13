using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody2D rb;
    private float pickUpRadius;
    private Transform playerTransform;
    private Item item;
    
    void Start()
    {
        if(gameObject.GetComponent<Item>() != null)
        {
            item = gameObject.GetComponent<Item>();
        }
        rb = GetComponent<Rigidbody2D>();
        pickUpRadius = Player.Instance.itemPickUpRadius; // Make sure your Player class has a public float field named pickUpRadius
        playerTransform = Player.Instance.transform; // Ensure your Player class has a Transform property or field accessible here
    }
    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if(distanceToPlayer <= pickUpRadius)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + directionToPlayer * Time.fixedDeltaTime); // You may want to multiply this by a speed variable
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (item != null)
            {
                Player.Instance.GainExp(item.expAmount);
                Player.Instance.GainHealth(item.healAmount);
            }
            if (item.expAmount > 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.gemSound, 0.15f, 0.1f);
            }

            if (item.healAmount > 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.flaskSound, 0.3f, 0.1f);
            }
            Destroy(gameObject); // Destroys this pickupable item
        }
    }
}
