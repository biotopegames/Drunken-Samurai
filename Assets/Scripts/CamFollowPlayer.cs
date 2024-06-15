using UnityEngine;
using Cinemachine;

public class CamFollowPlayer : MonoBehaviour
{
    // Reference to the Cinemachine Virtual Camera
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        // Get the Cinemachine Virtual Camera component attached to this GameObject
        vcam = GetComponent<CinemachineVirtualCamera>();

        // Check if the Cinemachine Virtual Camera component is found
        if (vcam == null)
        {
            Debug.LogError("Cinemachine Virtual Camera component not found.");
            return;
        }

        // Set the follow target to the Player GameObject
        if (Player.Instance != null)
        {
            vcam.Follow = Player.Instance.transform;
        }
        else
        {
            Debug.LogError("Player instance not found.");
        }
    }
}
