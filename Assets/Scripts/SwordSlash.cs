using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{

public Transform playerTransform;

void OnTriggerEnter2D(Collider2D other)
{
    if(other.CompareTag("Enemy"))
    {
        other.GetComponent<Enemy>().GetHurt(this.gameObject.GetComponentInParent<Player>().stats.damage);
        Vector2 attackDirection = other.transform.position - playerTransform.position;
        StartCoroutine(other.GetComponent<Enemy>().KnockBack(attackDirection, 0.01f));

    }
}

}
