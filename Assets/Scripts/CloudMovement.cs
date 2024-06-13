using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 1f;
    private float startX;
    private float endX;

    private void Start()
    {
        startX = transform.position.x;
        endX = startX - 10f; // Adjust this value to determine the end position
    }

    private void Update()
    {
        // Move the cloud to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Check if the cloud has reached the end position
        if (transform.position.x <= endX)
        {
            // Reset the cloud's position to the start position
            transform.position = new Vector2(startX, transform.position.y);
        }
    }
}
