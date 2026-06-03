using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the finish line is the Player
        if (collision.CompareTag("Player"))
        {
            // This instantly calls the NextLevel function automatically!
            SceneController.instance.NextLevel();
        }
    }
}