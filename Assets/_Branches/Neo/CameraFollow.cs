using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    public Transform player; // Drag Player
    public Vector3 offset = new Vector3(0, 2, -10); // Hauteur + distance

    [Header("Smooth")]
    public float smoothSpeed = 0.125f; // 0=instant, 1=lent

    void LateUpdate() // Après Player Update !
    {
        if (player == null) return;

        // Position idéale = Player + offset
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = offset.z; // Cam fixe Z

        // Smooth vers position idéale
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
