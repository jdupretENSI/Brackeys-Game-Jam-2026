using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    [Header("Checkpoint ID")]
    public int checkpointID = 0; // 0=début, 1=2e...

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player?.SetCheckpoint(checkpointID);
        }
    }
}