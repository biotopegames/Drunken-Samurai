using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugHelper : MonoBehaviour
{
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI movespeed;
    public TextMeshProUGUI cd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dmg.text = Player.Instance.stats.damage.ToString();
        hp.text = Player.Instance.stats.maxHealth.ToString();
        cd.text = Player.Instance.stats.dashCooldown.ToString();
        movespeed.text = Player.Instance.stats.moveSpeed.ToString();

    }
}
