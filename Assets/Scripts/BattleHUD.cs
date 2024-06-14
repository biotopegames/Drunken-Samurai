using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    private static BattleHUD instance;
    private Animator anim;
    public static BattleHUD Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<BattleHUD>();
            return instance;
        }
    }


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartBattle()
    {
        anim.SetTrigger("enter_battle");
    }

    public void EndBattle()
    {
        anim.SetTrigger("exit");
    }
}
