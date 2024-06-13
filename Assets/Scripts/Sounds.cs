using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    private AudioSource swordSlash;

    // Start is called before the first frame update
    void Start()
    {
        swordSlash = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void PlaySwordSound()
    {
        swordSlash.Play();
    }
}
