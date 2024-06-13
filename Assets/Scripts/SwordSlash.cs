using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{

void OnTriggerEnter2D(Collider2D other)
{
    if(other.CompareTag("Enemy"))
    {
        other.GetComponent<Enemy>().GetHurt(this.gameObject.GetComponentInParent<Player>().stats.damage);
    }
}

}
