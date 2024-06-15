using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Loads a new scene, while also clearing level-specific inventory!*/

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private PlayerPosition playerPosition;
    public float playerPosXInNextScene;
    public float playerPosYInNextScene;

    [SerializeField] string loadSceneName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == Player.Instance.gameObject)
        {
                // GameManager.Instance.hud.loadSceneName = loadSceneName;
                // GameManager.Instance.inventory.Clear();
                // GameManager.Instance.hud.animator.SetTrigger("coverScreen");
                // enabled = false;
                playerPosition.x = playerPosXInNextScene;
                playerPosition.y = playerPosYInNextScene;
                GameManager.Instance.LoadScene(loadSceneName);
            //transform.position = new Vector2 (playerPos.x, playerPos.y);

                //HUD.Instance.anim.SetTrigger("coverScreen");
                //StartCoroutine(WaitToLoad());
        }
    }


    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1);
        Player.Instance.transform.position = new Vector2(playerPosXInNextScene, playerPosYInNextScene);
        GameManager.Instance.LoadScene(loadSceneName);

    }

    private void Update()
    {



        // if(NewPlayer.Instance.isPressingE)
        // {
        //     hasPressedE = true;
        // }
        // else
        // {
        //     hasPressedE = false;
        // }
    }
}
