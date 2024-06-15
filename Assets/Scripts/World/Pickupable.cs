using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody2D rb;
    public float pickUpRadius;
    private Transform playerTransform;
    public bool isInventoryItem;
    private Consumable consumable;
    [SerializeField] private Item item;
    public int itemStackAmount;
    
    void Start()
    {
        if(!isInventoryItem)
        {
            consumable = gameObject.GetComponent<Consumable>();
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
            if (!isInventoryItem)
            {
                Player.Instance.GainExp(consumable.expAmount);
                Player.Instance.GainHealth(consumable.healAmount);
                Player.Instance.GainHonor(consumable.honorAmount);
                if (consumable.expAmount > 0)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.gemSound, 0.15f, 0.1f);
                }

                if (consumable.healAmount > 0)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.flaskSound, 0.3f, 0.1f);
                }
                Destroy(gameObject); // Destroys this pickupable item
            }
            else
            {
                Inventory.Instance.AddItem(item, itemStackAmount);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.gemSound, 0.15f, 0.1f);
                Destroy(gameObject); // Destroys this pickupable item
            }


        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue; // Set the color of the gizmo

    //     // Draw a wire sphere representing the pickup radius
    //     Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    // }
}
