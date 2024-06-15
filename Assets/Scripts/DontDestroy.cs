using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    void Awake()
    {
        // Check if an instance of the object already exists
        if (instance != null)
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Set this instance as the singleton instance
        instance = this;

        // Ensure that this object persists across scene changes
        DontDestroyOnLoad(gameObject);
    }
}